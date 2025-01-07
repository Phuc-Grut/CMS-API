using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.CMS.Models;

namespace VFi.Application.CMS.DTOs;

public class SortItemDto
{
    public Guid Id { get; set; }
    public int SortOrder { get; set; }
}
