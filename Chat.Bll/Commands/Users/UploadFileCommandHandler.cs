using Chat.Dal;
using MediatR;

namespace Chat.Bll.Commands.Users;

public record UploadFileCommand(string UserId,string SrcImage64) : IRequest<UploadFileCommandResult>;

public record UploadFileCommandResult(bool Success);

internal sealed class UploadFileCommandHandler : IRequestHandler<UploadFileCommand,UploadFileCommandResult>
{
    private readonly ChatContext _context;

    public UploadFileCommandHandler(ChatContext context)
    {
        _context = context;
    }
    public async Task<UploadFileCommandResult> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == new Guid(request.UserId));
        if (user != null)
        {
            user.PhotoBase64 = request.SrcImage64;
            await _context.SaveChangesAsync(CancellationToken.None);
            return new UploadFileCommandResult(true);
        }

        return new UploadFileCommandResult(false);
    }
}