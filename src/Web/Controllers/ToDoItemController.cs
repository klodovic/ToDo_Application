using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.TodoItems.Commands.CreateTodoItem;
using ToDoApp.Application.Common.Exceptions;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ToDoApp.Application.TodoItems.Commands.DeleteTodoItem;
using ToDoApp.Application.TodoItems.Commands.CompleteTodoItem;

namespace ToDoApp.Web.Controllers;

public class ToDoItemController : Controller
{
    private readonly ISender _sender;


    public ToDoItemController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            TempData["Errors"] = string.Join("\n", ex.Errors.Select(e => e.Value));
        }

        return Redirect("/");
    }



    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] List<int> ids, CancellationToken cancellationToken)
    {
        if (ids == null)
        {
            TempData["Errors"] = "No IDs provided for deletion.";
            return Redirect("/");
        }

        try
        {
            var command = new DeleteTodoItemsCommand { Ids = ids };
            await _sender.Send(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            TempData["Errors"] = string.Join("\n", ex.Errors.Select(e => e.Value));
        }

        return Redirect("/");
    }


    [HttpPost]
    public async Task<IActionResult> Complete([FromBody] List<int> ids, CancellationToken cancellationToken)
    {
        if (ids == null)
        {
            TempData["Errors"] = "No IDs provided for complete Todo tasks.";
            return Redirect("/");
        }

        try
        {
            var command = new CompleteTodoItemCommand { Ids = ids };
            await _sender.Send(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            TempData["Errors"] = string.Join("\n", ex.Errors.Select(e => e.Value));
        }

        return Redirect("/");
    }


}
