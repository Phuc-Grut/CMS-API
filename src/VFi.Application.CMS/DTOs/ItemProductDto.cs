using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.CMS.DTOs;

public class ItemProductDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
}
