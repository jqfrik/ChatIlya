using MediatR;

namespace Chat.Bll.Commands.Chats;

public record SendAttachmentsCommand() : IRequest<SendAttachmentsCommandResult>;

public record SendAttachmentsCommandResult();

internal sealed class SendAttachments : IRequestHandler<SendAttachmentsCommand,SendAttachmentsCommandResult>
{
    public Task<SendAttachmentsCommandResult> Handle(SendAttachmentsCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new SendAttachmentsCommandResult());
    }
}