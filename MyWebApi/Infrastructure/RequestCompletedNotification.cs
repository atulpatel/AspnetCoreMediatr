using MediatR;
using System;

namespace MembershipEligibilitySearch.Api.PipelineBehavior
{
    public class RequestCompletedNotification : INotification
    {
        public dynamic Request { get; set; }
        public dynamic Response { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}