using FplDashboard.API.Features.Players.Models;

namespace FplDashboard.API.Features.Players;

public class FilterChecker
{
    private readonly PlayerFilterRequest _request;
    private bool _isValid = true;

    private FilterChecker(PlayerFilterRequest request)
    {
        _request = request;
    }

    public static FilterChecker For(PlayerFilterRequest request) => new(request);

    public FilterChecker HasPositionFilter()
    {
        if (_isValid && (_request.PositionIds is null || _request.PositionIds.Count == 0))
            _isValid = false;
        return this;
    }

    public FilterChecker HasNoPositionFilter()
    {
        if (_isValid && _request.PositionIds is not null && _request.PositionIds.Count > 0)
            _isValid = false;
        return this;
    }

    public FilterChecker HasTeamFilter()
    {
        if (_isValid && (_request.TeamIds is null || _request.TeamIds.Count == 0))
            _isValid = false;
        return this;
    }

    public FilterChecker HasNoTeamFilter()
    {
        if (_isValid && _request.TeamIds is not null && _request.TeamIds.Count > 0)
            _isValid = false;
        return this;
    }

    public FilterChecker HasNoPlayerNameFilter()
    {
        if (_isValid && !string.IsNullOrEmpty(_request.PlayerName))
            _isValid = false;
        return this;
    }

    public FilterChecker HasNoMinutesFilter()
    {
        if (_isValid && _request.MinMinutes is not null)
            _isValid = false;
        return this;
    }

    public FilterChecker HasNoOrderBy()
    {
        if (_isValid && (!string.IsNullOrEmpty(_request.OrderBy) || !string.IsNullOrEmpty(_request.OrderDir)))
            _isValid = false;
        return this;
    }

    public FilterChecker HasDefaultPage()
    {
        if (_isValid && _request.PageSize != 20 && _request.Page != 1)
            _isValid = false;
        return this;
    }

    public bool IsMatch() => _isValid;
}