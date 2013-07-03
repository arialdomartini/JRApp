using System;
using System.Collections.Generic;
using JR.Core.Models.Twitter;

namespace JR.Core.Interfaces
{
    public interface ITwitterSearchProvider
    {
        void StartAsyncSearch(string searchText, Action<IEnumerable<Tweet>> success, Action<Exception> error);
    }
}