using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Application.Common.Interfaces;
using ToDoApp.Application.TodoItems.Commands.DeleteTodoItem;

namespace ToDoApp.Application.TodoItems.Commands.CompleteTodoItem;
public record CompleteTodoItemCommand : IRequest<bool>
{
    public List<int>? Ids { get; init; }
}

public class CompleteTodoItemCommandHandler : IRequestHandler<CompleteTodoItemCommand, bool>
{

    private readonly IToDoItemRepo _repo;
    private readonly IValidator<CompleteTodoItemCommandValidator> _validator;

    public CompleteTodoItemCommandHandler(IToDoItemRepo repo, IValidator<CompleteTodoItemCommandValidator> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<bool> Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync((IValidationContext)request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException((IEnumerable<FluentValidation.Results.ValidationFailure>)errors);
        }

        bool completed = true;
        if (request.Ids != null)
        {
            foreach (var id in request.Ids)
            {
                var todoItem = await _repo.GetByIdAsync(id, cancellationToken);
                if (todoItem != null)
                {
                    todoItem.Done = true;
                    await _repo.UpdateToDoItemAsync(todoItem, cancellationToken);
                }
                else
                {
                    completed = false;
                }
            }
        }
        else
        {
            completed = false;
        }
        return completed;   
    }
}
