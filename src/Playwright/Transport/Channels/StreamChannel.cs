/*
 * MIT License
 *
 * Copyright (c) Microsoft Corporation.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright.Core;

namespace Microsoft.Playwright.Transport.Channels;

internal class StreamChannel : Channel<Stream>
{
    public StreamChannel(string guid, Connection connection, Stream owner) : base(guid, connection, owner)
    {
    }

    internal async Task<byte[]> ReadAsync(int size)
    {
        var response = await Connection.SendMessageToServerAsync(
            Object,
            "read",
            new Dictionary<string, object>
            {
                ["size"] = size,
            }).ConfigureAwait(false);
        return response.Value.GetProperty("binary").GetBytesFromBase64();
    }

    internal Task CloseAsync() => Connection.SendMessageToServerAsync(Object, "close");
}
