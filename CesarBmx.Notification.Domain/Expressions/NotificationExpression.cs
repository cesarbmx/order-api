﻿using System;
using System.Linq.Expressions;
using CesarBmx.Notification.Domain.Models;

namespace CesarBmx.Notification.Domain.Expressions
{
    public static class NotificationExpression
    {
        public static Expression<Func<Message, bool>> Filter(string userId = null)
        {
            return x => string.IsNullOrEmpty(userId) || x.UserId == userId;
        }
        public static Expression<Func<Message, bool>> PendingNotification()
        {
            return x => !x.SentTime.HasValue;
        }
    }
}
