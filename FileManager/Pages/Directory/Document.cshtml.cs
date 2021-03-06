﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileManager.Models;
using FileManager.Models.Database.DepartmentsDocuments;
using FileManager.Models.Database.DocumentStatuses;
using FileManager.Models.Database.ReportingYearDocumentTitles;
using FileManager.Models.GetSummaryOfUploadedFilesAndChecks;
using FileManager.Services.DocumentManagerService;
using FileManager.Services.FileManagerService;
using FileManager.Services.GetAccountDataService;
using FileManager.Services.SmartBreadcrumbService;
using GemBox.Document;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FileManager.Models.Database.UserDepartmentRoles;
using System.Reflection;

namespace FileManager.Pages.Directory
{
    public class DocumentModel : PageModel
    {
        private readonly FileManagerContext db;
        private readonly ILogger<DocumentModel> _logger;
        private readonly ISmartBreadcrumbService _breadcrumbService;
        private readonly IGetAccountDataService _getAccountDataService;
        private readonly IFileManagerService _fileManagerService;
        private readonly IDocumentManagerService _documentManagerService;

        public DocumentTitle DocumentsTitle;
        public List<DepartmentsDocumentsVersion> UploadedDocuments;
        public List<DocumentStatusHistory> DocumentStatusHistories;
        public User user;
        public bool IsUserTheChecker = false;
        public string actualDocumentStatus;
        public List<DocumentStatus> AllAvailabledocumentStatuses;
        public DepartmentsDocument departmentsDocument;
        public Guid NewDocumentStatus;

        public Guid selectedReportingYearId;
        public Guid selectedDepartmentId;
        public Guid selectedDocumentTypeId;
        public Guid selectedDocumentTitleId;


        public DocumentModel(FileManagerContext context,
            ILogger<DocumentModel> logger,
            ISmartBreadcrumbService breadcrumbService,
            IGetAccountDataService getAccountDataService,
            IFileManagerService fileManagerService,
            IDocumentManagerService documentManagerService)
        {
            db = context;
            _logger = logger;
            _breadcrumbService = breadcrumbService;
            _getAccountDataService = getAccountDataService;
            _fileManagerService = fileManagerService;
            _documentManagerService = documentManagerService;

        }
        public async Task<IActionResult> OnGetAsync(Guid yearId, Guid departmentId, Guid documentTypeId, Guid documentTitleId)
        {
            try
            {
                if (!await _getAccountDataService.UserIsHaveAnyRoleOnDepartment(departmentId))
                {
                    return NotFound();
                }
                selectedReportingYearId = yearId;
                selectedDepartmentId = departmentId;
                selectedDocumentTypeId = documentTypeId;
                selectedDocumentTitleId = documentTitleId;

                user = await _getAccountDataService.GetCurrentUser();

                DocumentsTitle = await db.DocumentTitle.FirstOrDefaultAsync(dt => dt.Id.Equals(documentTitleId));

                IsUserTheChecker = await _getAccountDataService.UserIsCheckerOnDepartment(departmentId);

                AllAvailabledocumentStatuses = await db.DocumentStatus.ToListAsync();

                departmentsDocument = await _documentManagerService
                     .GetDepartmentsDocument(departmentId,
                         await _documentManagerService.GetCurrentReportingYearDocumentTitleId(yearId, documentTitleId));

                DocumentStatusHistories = await db.DocumentStatusHistory
                    .Include(dsh => dsh.DocumentStatus)
                    .Where(dd => dd.DepartmentsDocumentId == departmentsDocument.Id)
                    .OrderBy(dd => dd.SettingDateTime)
                    .Include(dsh => dsh.User)
                    .ToListAsync();

                UploadedDocuments = await db.DepartmentsDocumentsVersion
                    .Where(ddv => ddv.DepartmentDocumentId == departmentsDocument.Id)
                    .OrderByDescending(ddv => ddv.UploadedDateTime)
                    .Include(ddv => ddv.User)
                    .ToListAsync();
                var CurrentBreadCrumb = await _breadcrumbService.GetDocumentBreadCrumbNodeAsync(
                    yearId,
                    departmentId,
                    documentTypeId,
                    documentTitleId);
                ViewData["BreadcrumbNode"] = CurrentBreadCrumb;
                ViewData["Title"] = CurrentBreadCrumb.Title;

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting document page");
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostSaveFileAsync(IFormFile uploadedFile,
            Guid yearId,
            Guid departmentId,
            Guid documentTypeId,
            Guid documentTitleId,
           Guid departmentDocumentId,
            Guid userId)
        {
            try
            {
                if (uploadedFile != null)
                {

                    if (await _fileManagerService.UploadFileAsync(
                            uploadedFile,
                            yearId,
                            departmentId,
                            documentTitleId,
                            userId) > 0)
                    {
                        _logger.LogWarning("using danger code");
                        ////// !!! NOT STABLE CODE
                        var statusNotChecked = await db.DocumentStatus.FirstOrDefaultAsync(ds => ds.Status == "Не проверено");
                        ////// !!! NOT STABLE CODE

                        if (statusNotChecked == null)
                            return NotFound();

                        if (await AddDocumentStatusAsync(statusNotChecked.Id, "Документ загружен", departmentDocumentId, userId) > 0)
                        {
                            return RedirectToPage("Document", routeValues: new
                            {
                                yearId,
                                departmentId,
                                documentTypeId,
                                documentTitleId
                            });
                        }
                    }

                }
                return NotFound();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving document");
                return NotFound();
            }
        }
        private static byte[] GetBytes(GemBox.Document.DocumentModel document, SaveOptions options)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, options);
                return stream.ToArray();
            }
        }
        public async Task<IActionResult> OnPostGetSummaryOfUploadedFilesAndChecksAsync(Guid departmentsDocumentId)
        {
            try
            {

                List<SummaryOfUploadedFilesAndChecks> summary = new List<SummaryOfUploadedFilesAndChecks>();

                var DocumentStatusHistories = db.DocumentStatusHistory
                    .Where(dsh => dsh.DepartmentsDocumentId == departmentsDocumentId)
                    .Include(dsh => dsh.User);

                // Adding all statuses and time of their setting
                foreach (var documentStatusHistory in DocumentStatusHistories)
                {
                    var status = await db.DocumentStatus.FirstOrDefaultAsync(s => s.Id == documentStatusHistory.DocumentStatusId);
                    if (status != null)
                    {
                        summary.Add(new SummaryOfUploadedFilesAndChecks("Установлен статус " + status.Status,
                            documentStatusHistory.SettingDateTime,
                            documentStatusHistory.User.Email + " (" + documentStatusHistory.User.FistName + " " + documentStatusHistory.User.LastName + ")"));
                    }
                }

                var UploadedDocuments = db.DepartmentsDocumentsVersion
                    .Where(dsh => dsh.DepartmentDocumentId == departmentsDocumentId)
                    .Include(ddv => ddv.User);

                // Adding all uploads and time of their upload
                foreach (DepartmentsDocumentsVersion departmentsDocumentsVersion in UploadedDocuments)
                {
                    if (departmentsDocumentsVersion != null)
                    {
                        summary.Add(
                            new SummaryOfUploadedFilesAndChecks(
                                "Загружен документ " + departmentsDocumentsVersion.FileName,
                                departmentsDocumentsVersion.UploadedDateTime,
                                departmentsDocumentsVersion.User.Email + " (" + departmentsDocumentsVersion.User.FistName + " " + departmentsDocumentsVersion.User.LastName + ")"));
                    }
                }


                ComponentInfo.SetLicense("FREE-LIMITED-KEY");

                GemBox.Document.DocumentModel document = new GemBox.Document.DocumentModel();
                Section section = new Section(document);
                document.Sections.Add(section);

                var DepartmentDocument = (await db.DepartmentsDocument
                    .Include(rydt => rydt.Department)
                    .Include(dd => dd.ReportingYearDocumentTitle)
                        .ThenInclude(rydt => rydt.DocumentTitle)
                            .ThenInclude(dt => dt.DocumentType)
                    .Include(dd => dd.ReportingYearDocumentTitle)
                        .ThenInclude(dd => dd.ReportingYear)
                    .FirstOrDefaultAsync(dd => dd.Id == departmentsDocumentId));

                section.Blocks.Add(
                    new Paragraph(document,
                        new Run(document,
                            "Сводка загрузок и проверок по файлу "
                            + DepartmentDocument.ReportingYearDocumentTitle.ReportingYear.Number + "/"
                            + DepartmentDocument.Department.Name + "/"
                            + DepartmentDocument.ReportingYearDocumentTitle.DocumentTitle.DocumentType.Type + "/"
                            + DepartmentDocument.ReportingYearDocumentTitle.DocumentTitle.Name)
                        {
                            CharacterFormat = { Bold = true, Size = 24 }
                        }));
                foreach (var item in summary.OrderBy(s => s.DateTime))
                {
                    section.Blocks.Add(
                        new Paragraph(document,
                            new Run(document, item.Info + " | " + item.DateTime + " " + item.UploadedUser)));
                }


                SaveOptions options = SaveOptions.DocxDefault;

                return File(GetBytes(document, options), options.ContentType, "SummaryOfDownloadedFilesAndChecks.docx");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving document");
                return NotFound();
            }
        }
        public async Task<IActionResult> OnPostSetDocumentStatusAsync(Guid newStatusId, string comment, Guid departmentDocumentId,
            Guid yearId, Guid departmentId, Guid documentTypeId, Guid documentTitleId, Guid userId)
        {
            try
            {
                if (newStatusId != null)
                {

                    if (await AddDocumentStatusAsync(newStatusId, comment, departmentDocumentId, userId) > 0)
                    {
                        return RedirectToPage("Document", routeValues: new
                        {
                            yearId,
                            departmentId,
                            documentTypeId,
                            documentTitleId
                        });
                    }

                }
                return NotFound();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving document");
                return NotFound();
            }
        }
        private async Task<int> AddDocumentStatusAsync(Guid newStatusId, string comment, Guid departmentDocumentId, Guid userId)
        {
            var newStatus = new DocumentStatusHistory(newStatusId, comment, departmentDocumentId, DateTime.Now, userId);
            var UpdatedDepartamentDocument = (await db.DepartmentsDocument.FirstOrDefaultAsync(dd => dd.Id == departmentDocumentId));
            UpdatedDepartamentDocument.DocumentStatusHistories.Add(newStatus);
            UpdatedDepartamentDocument.LastSetDocumentStatusId = newStatusId;
            db.DepartmentsDocument.Update(UpdatedDepartamentDocument);


            return await db.SaveChangesAsync();
        }
        public async Task<PhysicalFileResult> OnGetDownloadDocumentAsync(Guid departmentsDocumentsVersionId)
        {
            DepartmentsDocumentsVersion departmentsDocumentsVersion = await db.DepartmentsDocumentsVersion
                .FirstOrDefaultAsync(ddv => ddv.Id == departmentsDocumentsVersionId);
            var filepath = departmentsDocumentsVersion.Path;

            return PhysicalFile(filepath, "application/octet-stream", departmentsDocumentsVersion.FileName);
        }
    }
}