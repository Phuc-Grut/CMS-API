

using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.CMS.Models;

namespace VFi.Application.CMS.DTOs;

public class ContentTypeDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
