using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Application.TodoItems.Commands.DeleteTodoItem;
public class DeleteTodoItemsCommandValidator : AbstractValidator<DeleteTodoItemsCommand>
{
    public DeleteTodoItemsCommandValidator() 
    {
        RuleFor(v => v.Ids)
            .NotEmpty().WithMessage("List of IDs cannot be empty.");
    }
}
