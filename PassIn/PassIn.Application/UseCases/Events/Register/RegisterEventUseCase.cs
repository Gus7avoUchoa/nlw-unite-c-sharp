using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register;

public class RegisterEventUseCase
{
    private readonly PassInDbContext _dbContext;

    public RegisterEventUseCase()
    {
        _dbContext = new PassInDbContext();
    }

    public ResponseEventJson Execute(RequestEventJson request)
    {
        Validate(request);

        var entity = new Infrastructure.Entities.Event
        {   
            Title = request.Title,
            Details = request.Details,
            Maximum_Attendees = request.MaximumAttendees,
            Slug = request.Title.ToLower().Replace(" ", "-"),
        };

        _dbContext.Events.Add(entity);
        _dbContext.SaveChanges();

        return new ResponseEventJson
        {
            Id = entity.Id,
            Title = entity.Title,
            Details = entity.Details,
            MaximumAttendees = entity.Maximum_Attendees,
            AttendeesAmount = 0, // TODO: Implement this
        };
    }

    public static void Validate(RequestEventJson request)
    {
        if (request.MaximumAttendees <= 0)
            throw new ErrorOnValidationException("The Maximum attendes is invalid.");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ErrorOnValidationException("The title is invalid.");

        if (string.IsNullOrWhiteSpace(request.Details))
            throw new ErrorOnValidationException("The title is invalid.");
    }
}