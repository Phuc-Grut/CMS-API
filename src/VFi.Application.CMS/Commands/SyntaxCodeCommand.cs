using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.CMS.Commands.Validations;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class SyntaxCodeCommand : Command
{
}

public class UseCodeCommand : SyntaxCodeCommand
{
    public string Syntax { get; set; }
    public string Code { get; set; }

    public UseCodeCommand(string syntax, string code)
    {
        Syntax = syntax;
        Code = code;
    }
}
