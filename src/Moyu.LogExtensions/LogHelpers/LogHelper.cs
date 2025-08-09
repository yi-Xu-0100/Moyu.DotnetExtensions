// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

using Microsoft.Extensions.Logging;

namespace Moyu.LogExtensions.LogHelpers;

/// <summary>
/// 日志辅助类用于格式化日志消息
/// </summary>
public static partial class LogHelper
{
    #region Trace overload

    /// <summary>
    /// 记录<see cref="LogLevel.Trace"/>日志
    /// </summary>
    /// <param name="logger">日志对象</param>
    /// <param name="message">日志消息</param>
    [LoggerMessage(Level = LogLevel.Trace, Message = "{message}")]
    public static partial void Trace(this ILogger logger, string message);

    #endregion

    #region Debug overload

    /// <summary>
    /// 记录<see cref="LogLevel.Debug"/>日志
    /// </summary>
    /// <param name="logger">日志对象</param>
    /// <param name="message">日志消息</param>
    [LoggerMessage(Level = LogLevel.Debug, Message = "{message}")]
    public static partial void Debug(this ILogger logger, string message);

    #endregion

    #region Info overload

    /// <summary>
    /// 记录<see cref="LogLevel.Information"/>日志
    /// </summary>
    /// <param name="logger">日志对象</param>
    /// <param name="message">日志消息</param>
    [LoggerMessage(Level = LogLevel.Information, Message = "{message}")]
    public static partial void Info(this ILogger logger, string message);

    #endregion

    #region Warn overload

    /// <summary>
    /// 记录<see cref="LogLevel.Warning"/>日志
    /// </summary>
    /// <param name="logger">日志对象</param>
    /// <param name="message">日志消息</param>
    [LoggerMessage(Level = LogLevel.Warning, Message = "{message}")]
    public static partial void Warn(this ILogger logger, string message);

    /// <summary>
    /// 记录<see cref="LogLevel.Warning"/>日志
    /// </summary>
    /// <param name="logger">日志对象</param>
    /// <param name="ex">异常对象</param>
    /// <param name="message">日志消息</param>
    [LoggerMessage(Level = LogLevel.Warning, Message = "{message}")]
    public static partial void Warn(this ILogger logger, Exception ex, string message);

    #endregion

    #region Error overload

    /// <summary>
    /// 记录<see cref="LogLevel.Error"/>日志
    /// </summary>
    /// <param name="logger">日志对象</param>
    /// <param name="message">日志消息</param>
    [LoggerMessage(Level = LogLevel.Error, Message = "{message}")]
    public static partial void Error(this ILogger logger, string message);

    /// <summary>
    /// 记录<see cref="LogLevel.Error"/>日志
    /// </summary>
    /// <param name="logger">日志对象</param>
    /// <param name="ex">异常对象</param>
    /// <param name="message">日志消息</param>
    [LoggerMessage(Level = LogLevel.Error, Message = "{message}")]
    public static partial void Error(this ILogger logger, Exception ex, string message);

    #endregion

    #region Critical overload

    /// <summary>
    /// 记录<see cref="LogLevel.Critical"/>日志
    /// </summary>
    /// <param name="logger">日志对象</param>
    /// <param name="message">日志消息</param>
    [LoggerMessage(Level = LogLevel.Critical, Message = "{message}")]
    public static partial void Fatal(this ILogger logger, string message);

    /// <summary>
    /// 记录<see cref="LogLevel.Critical"/>日志
    /// </summary>
    /// <param name="logger">日志对象</param>
    /// <param name="ex">异常对象</param>
    /// <param name="message">日志消息</param>
    [LoggerMessage(Level = LogLevel.Critical, Message = "{message}")]
    public static partial void Fatal(this ILogger logger, Exception ex, string message);

    #endregion
}
