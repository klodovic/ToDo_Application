using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Interfaces;
public interface IToDoItemRepo
{
    Task AddNewToDoItemAsync(TodoItem item, CancellationToken cancellationToken);
    Task<TodoItem> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task UpdateToDoItemAsync(TodoItem item, CancellationToken cancellationToken);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
}
