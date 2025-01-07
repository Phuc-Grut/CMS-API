using FluentValidation.Results;
using MediatR;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands.Handler;

internal class SyntaxCodeCommandHandler : CommandHandler, IRequestHandler<UseCodeCommand, ValidationResult>
{
    private readonly ISyntaxCodeRepository _syntaxCodeRepository;

    public SyntaxCodeCommandHandler(ISyntaxCodeRepository syntaxCodeRepository)
    {
        _syntaxCodeRepository = syntaxCodeRepository;
    }
    //public void Dispose()
    //{
    //    _syntaxCodeRepository.Dispose();
    //}

    public async Task<ValidationResult> Handle(UseCodeCommand request, CancellationToken cancellationToken)
    {
        _syntaxCodeRepository.UseCode(request.Syntax, request.Code);
        return new ValidationResult();
    }

}
