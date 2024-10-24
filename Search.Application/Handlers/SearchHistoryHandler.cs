﻿using MediatR;
using SearchBarWebAPI.Search.Application.Queries;
using SearchBarWebAPI.Search.Core.Interface;
using SearchBarWebAPI.Search.Core.Model;

namespace SearchBarWebAPI.Search.Application.Handlers
{
    public class SearchHistoryHandler : IRequestHandler<SearchHistoryQuery, List<SearchHistory>>
    {
        private readonly ISearchHistoryRepository _searchHistoryRepository;

        public SearchHistoryHandler(ISearchHistoryRepository searchHistoryRepository)
        {
            _searchHistoryRepository = searchHistoryRepository;
        }

        public async Task<List<SearchHistory>> Handle(SearchHistoryQuery request, CancellationToken cancellationToken)
        {
            return (List<SearchHistory>)await _searchHistoryRepository.GetSearchHistoryAsync(request.UserId);
        }
    }
}
