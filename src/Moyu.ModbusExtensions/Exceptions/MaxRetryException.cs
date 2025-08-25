// Copyright © 2025-present yi-Xu-0100.
// This file is licensed under the MIT License. See LICENSE for details.

namespace Moyu.ModbusExtensions.Exceptions;

using System;

/// <summary>
/// 表示操作已达到最大重试次数时抛出的异常
/// </summary>
[Serializable]
public class MaxRetryException : InvalidOperationException
{
    /// <summary>
    /// 获取允许的最大重试次数
    /// </summary>
    public int MaxRetryCount { get; }

    /// <summary>
    /// 默认构造函数，最大重试次数默认为 0
    /// </summary>
    public MaxRetryException()
        : this(0, "已达到最大重试次数")
    {
    }

    /// <summary>
    /// 使用指定的最大重试次数初始化
    /// </summary>
    /// <param name="maxRetryCount">允许的最大重试次数</param>
    public MaxRetryException(int maxRetryCount)
        : this(maxRetryCount, $"已达到最大重试次数：{maxRetryCount}")
    {
    }

    /// <summary>
    /// 使用指定的最大重试次数和自定义异常消息初始化
    /// </summary>
    /// <param name="maxRetryCount">允许的最大重试次数</param>
    /// <param name="message">异常消息</param>
    public MaxRetryException(int maxRetryCount, string message)
        : base(message)
    {
        MaxRetryCount = maxRetryCount;
    }

    /// <summary>
    /// 使用指定的最大重试次数、自定义异常消息和内部异常初始化
    /// </summary>
    /// <param name="maxRetryCount">允许的最大重试次数</param>
    /// <param name="message">异常消息</param>
    /// <param name="innerException">内部异常</param>
    public MaxRetryException(int maxRetryCount, string message, Exception innerException)
        : base(message, innerException)
    {
        MaxRetryCount = maxRetryCount;
    }
}
