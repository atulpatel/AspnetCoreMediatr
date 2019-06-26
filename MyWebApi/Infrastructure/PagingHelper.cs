namespace MembershipEligibilitySearch.Api.Infrastructure
{
    using MyWebApi.Infrastructure;
    using System;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Defines the <see cref="PagingHelper" />
    /// </summary>
    public static class PagingHelper
    {
        /// <summary>
        /// The GetLocation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiResponse">The apiResponse<see cref="ApiResponse{T}"/></param>
        /// <param name="offset">The offset<see cref="int"/></param>
        /// <param name="limit">The limit<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        /// <param name="urlWithFormatter">The urlwithFormatter like "/api/controller/action/offset/{0}/limit/{1}?" <see cref="string"/></param>
        /// <returns>The <see cref="ApiResponse{T}"/></returns>
        public static ApiResponse<T> GetLocation<T>(this ApiResponse<T> apiResponse, int offset, int limit, int count, string urlWithFormatter)
        {
            var location = new Location
            {
                Limit = limit,
                Total = count,
                Offset = offset
            };

            var nextUrl = "";
            if ((offset + 1) * limit < location.Total)
            {
                var nextOffset = offset + 1;
                nextUrl = string.Format(urlWithFormatter, nextOffset, limit);
            }

            var prevUrl = "";
            if (offset > 0)
            {
                var nextOffset = offset - 1;
                prevUrl = string.Format(urlWithFormatter, nextOffset, limit);
            }

            location.Next = nextUrl;
            location.Previous = prevUrl;

            apiResponse.Location = location;

            return apiResponse;
        }

        /// <summary>
        /// The GetQueryString
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where (p.Name != "Limit" && p.Name != "Offset") && p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }
}
