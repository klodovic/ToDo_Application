using FluentValidation;
using ToDoApp.Application.Common.Interfaces;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; init; }

    public string? Title { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IToDoItemRepo _repo;
    private readonly IValidator<CreateTodoItemCommandValidator> _validator;
    //private readonly IMapper _mapper;

    public CreateTodoItemCommandHandler(IToDoItemRepo repo, IValidator<CreateTodoItemCommandValidator> validator)
    {
        _repo = repo;
        _validator = validator;
        //_mapper = mapper;
    }

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {

        var validationResult = await _validator.ValidateAsync((IValidationContext)request, cancellationToken);
        if (!validationResult.IsValid) {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException((IEnumerable<FluentValidation.Results.ValidationFailure>)errors);
        }

        var todoItem = new TodoItem { Title = request.Title, Done = false };
        await _repo.AddNewToDoItemAsync(todoItem, cancellationToken);

        return todoItem.Id;
    }
}
