using MediatR;

namespace ActionServiceAPI.Application.Commands.DeleteActionCommand
{
    public class DeleteActionCommand(int id) : IRequest<bool>
    {
        public int Id { get; init; } = id;
    }
}
