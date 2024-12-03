using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Application.TodoItems.Commands.CompleteTodoItem;
public class CompleteTodoItemCommandValidator : AbstractValidator<CompleteTodoItemCommand>
{
    public CompleteTodoItemCommandValidator()
    {
        RuleFor(v => v.Ids)
            .NotEmpty().WithMessage("List of IDs cannot be empty.");
    }
}
