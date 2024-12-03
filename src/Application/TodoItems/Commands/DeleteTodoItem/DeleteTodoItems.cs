using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.TodoItems.Commands.DeleteTodoItem;
public record DeleteTodoItemsCommand : IRequest
{
    public List<int>? Ids { get; set; }
}

public class DeleteTodoItemsCommandHandler : IRequestHandler<DeleteTodoItemsCommand>
{

    private readonly IToDoItemRepo _repo;
    private readonly IValidator<DeleteTodoItemsCommandValidator> _validator;

    public DeleteTodoItemsCommandHandler(IToDoItemRepo repo, IValidator<DeleteTodoItemsCommandValidator> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task Handle(DeleteTodoItemsCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync((IValidationContext)request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException((IEnumerable<FluentValidation.Results.ValidationFailure>)errors);
        }

        if (request.Ids != null)
        {
            foreach (var id in request.Ids)
            {
                var todoItem = await _repo.GetByIdAsync(id, cancellationToken);
                if (todoItem != null)
                {
                    await _repo.DeleteByIdAsync(id, cancellationToken);
                }
            }
        }

        return;
    }
}
