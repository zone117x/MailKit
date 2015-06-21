//
// ImapFolderAsync.cs
//
// Author: Jeffrey Stedfast <jeff@xamarin.com>
//
// Copyright (c) 2015 Xamarin Inc. (www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using MimeKit;
using MailKit.Search;

namespace MailKit.Net.Imap {
	public partial class ImapFolder
	{
		/// <summary>
		/// Appends the specified message to the folder.
		/// </summary>
		/// <remarks>
		/// Appends the specified message to the folder and returns the UniqueId assigned to the message.
		/// </remarks>
		/// <returns>The UID of the appended message, if available; otherwise, <c>null</c>.</returns>
		/// <param name="message">The message.</param>
		/// <param name="flags">The message flags.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="progress">The progress reporting mechanism.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <para><paramref name="message"/> is <c>null</c>.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// Internationalized formatting was requested but has not been enabled.
		/// </exception>
		/// <exception cref="FolderNotFoundException">
		/// The <see cref="ImapFolder"/> does not exist.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// Internationalized formatting was requested but is not supported by the server.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<UniqueId?> AppendAsync (MimeMessage message, MessageFlags flags = MessageFlags.None, CancellationToken cancellationToken = default (CancellationToken), ITransferProgress progress = null)
		{
			return await AppendAsync (FormatOptions.Default, message, flags, cancellationToken, progress);
		}

		/// <summary>
		/// Appends the specified message to the folder.
		/// </summary>
		/// <remarks>
		/// Appends the specified message to the folder and returns the UniqueId assigned to the message.
		/// </remarks>
		/// <returns>The UID of the appended message, if available; otherwise, <c>null</c>.</returns>
		/// <param name="message">The message.</param>
		/// <param name="flags">The message flags.</param>
		/// <param name="date">The received date of the message.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="progress">The progress reporting mechanism.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <para><paramref name="message"/> is <c>null</c>.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// Internationalized formatting was requested but has not been enabled.
		/// </exception>
		/// <exception cref="FolderNotFoundException">
		/// The <see cref="ImapFolder"/> does not exist.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// Internationalized formatting was requested but is not supported by the server.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<UniqueId?> AppendAsync (MimeMessage message, MessageFlags flags, DateTimeOffset date, CancellationToken cancellationToken = default (CancellationToken), ITransferProgress progress = null)
		{
			return await AppendAsync (FormatOptions.Default, message, flags, date, cancellationToken, progress);
		}

		/// <summary>
		/// Appends the specified messages to the folder.
		/// </summary>
		/// <remarks>
		/// Appends the specified messages to the folder and returns the UniqueIds assigned to the messages.
		/// </remarks>
		/// <returns>The UIDs of the appended messages, if available; otherwise an empty array.</returns>
		/// <param name="messages">The list of messages to append to the folder.</param>
		/// <param name="flags">The message flags to use for each message.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="progress">The progress reporting mechanism.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <para><paramref name="messages"/> is <c>null</c>.</para>
		/// <para>-or-</para>
		/// <para><paramref name="flags"/> is <c>null</c>.</para>
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para>One or more of the <paramref name="messages"/> is null.</para>
		/// <para>-or-</para>
		/// <para>The number of messages does not match the number of flags.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// Internationalized formatting was requested but has not been enabled.
		/// </exception>
		/// <exception cref="FolderNotFoundException">
		/// The <see cref="ImapFolder"/> does not exist.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// Internationalized formatting was requested but is not supported by the server.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<IList<UniqueId>> AppendAsync (IList<MimeMessage> messages, IList<MessageFlags> flags, CancellationToken cancellationToken = default (CancellationToken), ITransferProgress progress = null)
		{
			return await AppendAsync (FormatOptions.Default, messages, flags, cancellationToken, progress);
		}

		/// <summary>
		/// Appends the specified messages to the folder.
		/// </summary>
		/// <remarks>
		/// Appends the specified messages to the folder and returns the UniqueIds assigned to the messages.
		/// </remarks>
		/// <returns>The UIDs of the appended messages, if available; otherwise an empty array.</returns>
		/// <param name="messages">The list of messages to append to the folder.</param>
		/// <param name="flags">The message flags to use for each of the messages.</param>
		/// <param name="dates">The received dates to use for each of the messages.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="progress">The progress reporting mechanism.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <para><paramref name="messages"/> is <c>null</c>.</para>
		/// <para>-or-</para>
		/// <para><paramref name="flags"/> is <c>null</c>.</para>
		/// <para>-or-</para>
		/// <para><paramref name="dates"/> is <c>null</c>.</para>
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para>One or more of the <paramref name="messages"/> is null.</para>
		/// <para>-or-</para>
		/// <para>The number of messages, flags, and dates do not match.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// Internationalized formatting was requested but has not been enabled.
		/// </exception>
		/// <exception cref="FolderNotFoundException">
		/// The <see cref="ImapFolder"/> does not exist.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// Internationalized formatting was requested but is not supported by the server.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<IList<UniqueId>> AppendAsync (IList<MimeMessage> messages, IList<MessageFlags> flags, IList<DateTimeOffset> dates, CancellationToken cancellationToken = default (CancellationToken), ITransferProgress progress = null)
		{
			return await AppendAsync (FormatOptions.Default, messages, flags, dates, cancellationToken, progress);
		}

		/// <summary>
		/// Copy the specified message to the destination folder.
		/// </summary>
		/// <remarks>
		/// Copies the specified message to the destination folder.
		/// </remarks>
		/// <returns>The UID of the message in the destination folder, if available; otherwise, <c>null</c>.</returns>
		/// <param name="uid">The UID of the message to copy.</param>
		/// <param name="destination">The destination folder.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="destination"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uid"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>The destination folder does not belong to the <see cref="ImapClient"/>.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotFoundException">
		/// <paramref name="destination"/> does not exist.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The IMAP server does not support the UIDPLUS extension.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<UniqueId?> CopyToAsync (UniqueId uid, IMailFolder destination, CancellationToken cancellationToken = default (CancellationToken))
		{
			var uids = await CopyToAsync (new [] { uid }, destination, cancellationToken);

			if (uids != null && uids.Count > 0)
				return uids[0];

			return null;
		}

		/// <summary>
		/// Move the specified message to the destination folder.
		/// </summary>
		/// <remarks>
		/// <para>Moves the specified messages to the destination folder.</para>
		/// <para>If the IMAP server supports the MOVE extension (check the <see cref="ImapClient.Capabilities"/>
		/// property for the <see cref="ImapCapabilities.Move"/> flag), then this operation will be atomic.
		/// Otherwise, MailKit implements this by first copying the messages to the destination folder, then
		/// marking them for deletion in the originating folder, and finally expunging them (see
		/// <see cref="Expunge(IList&lt;UniqueId&gt;,CancellationToken)"/> for more information about how a
		/// subset of messages are expunged). Since the server could disconnect at any point between those 3
		/// (or more) commands, it is advisable for clients to implement their own logic for moving messages when
		/// the IMAP server does not support the MOVE command in order to better handle spontanious server
		/// disconnects and other error conditions.</para>
		/// </remarks>
		/// <returns>The UID of the message in the destination folder, if available; otherwise, <c>null</c>.</returns>
		/// <param name="uid">The UID of the message to move.</param>
		/// <param name="destination">The destination folder.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="destination"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uid"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>The destination folder does not belong to the <see cref="ImapClient"/>.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotFoundException">
		/// <paramref name="destination"/> does not exist.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<UniqueId?> MoveToAsync (UniqueId uid, IMailFolder destination, CancellationToken cancellationToken = default (CancellationToken))
		{
			var uids = await MoveToAsync (new [] { uid }, destination, cancellationToken);

			if (uids != null && uids.Count > 0)
				return uids[0];

			return null;
		}

		/// <summary>
		/// Copy the specified message to the destination folder.
		/// </summary>
		/// <remarks>
		/// Copies the specified message to the destination folder.
		/// </remarks>
		/// <param name="index">The index of the message to copy.</param>
		/// <param name="destination">The destination folder.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="destination"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="index"/> does not refer to a valid message index.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// The destination folder does not belong to the <see cref="ImapClient"/>.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotFoundException">
		/// <paramref name="destination"/> does not exist.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The IMAP server does not support the UIDPLUS extension.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task CopyToAsync (int index, IMailFolder destination, CancellationToken cancellationToken = default (CancellationToken))
		{
			await CopyToAsync (new [] { index }, destination, cancellationToken);
		}

		/// <summary>
		/// Move the specified message to the destination folder.
		/// </summary>
		/// <remarks>
		/// Moves the specified message to the destination folder.
		/// </remarks>
		/// <param name="index">The index of the message to move.</param>
		/// <param name="destination">The destination folder.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="destination"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="index"/> does not refer to a valid message index.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// The destination folder does not belong to the <see cref="ImapClient"/>.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotFoundException">
		/// <paramref name="destination"/> does not exist.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The IMAP server does not support the UIDPLUS extension.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task MoveToAsync (int index, IMailFolder destination, CancellationToken cancellationToken = default (CancellationToken))
		{
			await MoveToAsync (new [] { index }, destination, cancellationToken);
		}

		/// <summary>
		/// Gets a substream of the specified body part.
		/// </summary>
		/// <remarks>
		/// <para>Gets a substream of the specified body part.</para>
		/// </remarks>
		/// <returns>The stream.</returns>
		/// <param name="uid">The UID of the message.</param>
		/// <param name="part">The desired body part of the message.</param>
		/// <param name="offset">The starting offset of the first desired byte.</param>
		/// <param name="count">The number of bytes desired.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="progress">The progress reporting mechanism.</param>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="uid"/> is invalid.
		/// </exception>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="part"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<Stream> GetStreamAsync (UniqueId uid, BodyPart part, int offset, int count, CancellationToken cancellationToken = default(CancellationToken), ITransferProgress progress = null)
		{
			return await GetStreamAsync (uid, part.PartSpecifier, offset, count, cancellationToken, progress);
		}

		/// <summary>
		/// Gets a substream of the specified body part.
		/// </summary>
		/// <remarks>
		/// <para>Gets a substream of the specified body part.</para>
		/// </remarks>
		/// <returns>The stream.</returns>
		/// <param name="index">The index of the message.</param>
		/// <param name="part">The desired body part of the message.</param>
		/// <param name="offset">The starting offset of the first desired byte.</param>
		/// <param name="count">The number of bytes desired.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="progress">The progress reporting mechanism.</param>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="index"/> is invalid.
		/// </exception>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="part"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<Stream> GetStreamAsync (int index, BodyPart part, int offset, int count, CancellationToken cancellationToken = default(CancellationToken), ITransferProgress progress = null)
		{
			return await GetStreamAsync (index, part.PartSpecifier, offset, count, cancellationToken, progress);
		}

		/// <summary>
		/// Add a set of flags to the specified message.
		/// </summary>
		/// <remarks>
		/// Adds a set of flags to the specified message.
		/// </remarks>
		/// <param name="uid">The UID of the message.</param>
		/// <param name="flags">The message flags to add.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uid"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task AddFlagsAsync (UniqueId uid, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await AddFlagsAsync (new [] { uid }, flags, silent, cancellationToken);
		}

		/// <summary>
		/// Add a set of flags to the specified message.
		/// </summary>
		/// <remarks>
		/// Adds a set of flags to the specified message.
		/// </remarks>
		/// <param name="uid">The UID of the message.</param>
		/// <param name="flags">The message flags to add.</param>
		/// <param name="userFlags">A set of user-defined flags to add.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uid"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task AddFlagsAsync (UniqueId uid, MessageFlags flags, HashSet<string> userFlags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await AddFlagsAsync (new [] { uid }, flags, userFlags, silent, cancellationToken);
		}

		/// <summary>
		/// Add a set of flags to the specified messages.
		/// </summary>
		/// <remarks>
		/// Adds a set of flags to the specified messages.
		/// </remarks>
		/// <param name="uids">The UIDs of the messages.</param>
		/// <param name="flags">The message flags to add.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="uids"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uids"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="uids"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task AddFlagsAsync (IList<UniqueId> uids, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await AddFlagsAsync (uids, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Remove a set of flags to the specified message.
		/// </summary>
		/// <remarks>
		/// Removes a set of flags to the specified message.
		/// </remarks>
		/// <param name="uid">The UID of the message.</param>
		/// <param name="flags">The message flags to remove.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uid"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task RemoveFlagsAsync (UniqueId uid, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await RemoveFlagsAsync (new [] { uid }, flags, silent, cancellationToken);
		}

		/// <summary>
		/// Add a set of flags to the specified message.
		/// </summary>
		/// <remarks>
		/// Adds a set of flags to the specified message.
		/// </remarks>
		/// <param name="uid">The UID of the message.</param>
		/// <param name="flags">The message flags to remove.</param>
		/// <param name="userFlags">A set of user-defined flags to remove.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uid"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task RemoveFlagsAsync (UniqueId uid, MessageFlags flags, HashSet<string> userFlags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await RemoveFlagsAsync (new [] { uid }, flags, userFlags, silent, cancellationToken);
		}

		/// <summary>
		/// Remove a set of flags from the specified messages.
		/// </summary>
		/// <remarks>
		/// Removes a set of flags from the specified messages.
		/// </remarks>
		/// <param name="uids">The UIDs of the messages.</param>
		/// <param name="flags">The message flags to remove.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="uids"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uids"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="uids"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task RemoveFlagsAsync (IList<UniqueId> uids, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await RemoveFlagsAsync (uids, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Set the flags of the specified message.
		/// </summary>
		/// <remarks>
		/// Sets the flags of the specified message.
		/// </remarks>
		/// <param name="uid">The UIDs of the message.</param>
		/// <param name="flags">The message flags to set.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="uid"/> is invalid.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task SetFlagsAsync (UniqueId uid, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await SetFlagsAsync (new [] { uid }, flags, silent, cancellationToken);
		}

		/// <summary>
		/// Set the flags of the specified message.
		/// </summary>
		/// <remarks>
		/// Sets the flags of the specified message.
		/// </remarks>
		/// <param name="uid">The UIDs of the message.</param>
		/// <param name="flags">The message flags to set.</param>
		/// <param name="userFlags">A set of user-defined flags to set.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="uid"/> is invalid.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task SetFlagsAsync (UniqueId uid, MessageFlags flags, HashSet<string> userFlags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await SetFlagsAsync (new [] { uid }, flags, userFlags, silent, cancellationToken);
		}

		/// <summary>
		/// Set the flags of the specified messages.
		/// </summary>
		/// <remarks>
		/// Sets the flags of the specified messages.
		/// </remarks>
		/// <param name="uids">The UIDs of the messages.</param>
		/// <param name="flags">The message flags to set.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="uids"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uids"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="uids"/> is invalid.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task SetFlagsAsync (IList<UniqueId> uids, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await SetFlagsAsync (uids, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Adds a set of flags to the specified messages only if their mod-sequence value is less than the specified value.
		/// </summary>
		/// <remarks>
		/// Adds a set of flags to the specified messages only if their mod-sequence value is less than the specified value.
		/// </remarks>
		/// <returns>The unique IDs of the messages that were not updated.</returns>
		/// <param name="uids">The UIDs of the messages.</param>
		/// <param name="modseq">The mod-sequence value.</param>
		/// <param name="flags">The message flags to add.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="uids"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uids"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="uids"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The <see cref="ImapFolder"/> does not support mod-sequences.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<IList<UniqueId>> AddFlagsAsync (IList<UniqueId> uids, ulong modseq, MessageFlags flags, bool silent, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await AddFlagsAsync (uids, modseq, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Removes a set of flags from the specified messages only if their mod-sequence value is less than the specified value.
		/// </summary>
		/// <remarks>
		/// Removes a set of flags from the specified messages only if their mod-sequence value is less than the specified value.
		/// </remarks>
		/// <returns>The unique IDs of the messages that were not updated.</returns>
		/// <param name="uids">The UIDs of the messages.</param>
		/// <param name="modseq">The mod-sequence value.</param>
		/// <param name="flags">The message flags to remove.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="uids"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uids"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="uids"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The <see cref="ImapFolder"/> does not support mod-sequences.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<IList<UniqueId>> RemoveFlagsAsync (IList<UniqueId> uids, ulong modseq, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			return await RemoveFlagsAsync (uids, modseq, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Sets the flags of the specified messages only if their mod-sequence value is less than the specified value.
		/// </summary>
		/// <remarks>
		/// Sets the flags of the specified messages only if their mod-sequence value is less than the specified value.
		/// </remarks>
		/// <returns>The unique IDs of the messages that were not updated.</returns>
		/// <param name="uids">The UIDs of the messages.</param>
		/// <param name="modseq">The mod-sequence value.</param>
		/// <param name="flags">The message flags to set.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="uids"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uids"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="uids"/> is invalid.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The <see cref="ImapFolder"/> does not support mod-sequences.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<IList<UniqueId>> SetFlagsAsync (IList<UniqueId> uids, ulong modseq, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			return await SetFlagsAsync (uids, modseq, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Add a set of flags to the specified message.
		/// </summary>
		/// <remarks>
		/// Adds a set of flags to the specified message.
		/// </remarks>
		/// <param name="index">The index of the message.</param>
		/// <param name="flags">The message flags to add.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="index"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="IMailStore"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="IMailStore"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="IMailStore"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task AddFlagsAsync (int index, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await AddFlagsAsync (new [] { index }, flags, silent, cancellationToken);
		}

		/// <summary>
		/// Add a set of flags to the specified message.
		/// </summary>
		/// <remarks>
		/// Adds a set of flags to the specified message.
		/// </remarks>
		/// <param name="index">The index of the message.</param>
		/// <param name="flags">The message flags to add.</param>
		/// <param name="userFlags">A set of user-defined flags to add.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="index"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="IMailStore"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="IMailStore"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="IMailStore"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task AddFlagsAsync (int index, MessageFlags flags, HashSet<string> userFlags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await AddFlagsAsync (new [] { index }, flags, userFlags, silent, cancellationToken);
		}

		/// <summary>
		/// Add a set of flags to the specified messages.
		/// </summary>
		/// <remarks>
		/// Adds a set of flags to the specified messages.
		/// </remarks>
		/// <param name="indexes">The indexes of the messages.</param>
		/// <param name="flags">The message flags to add.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="indexes"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="indexes"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="indexes"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="IMailStore"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="IMailStore"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="IMailStore"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The folder is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task AddFlagsAsync (IList<int> indexes, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await AddFlagsAsync (indexes, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Adds a set of flags to the specified messages only if their mod-sequence value is less than the specified value.
		/// </summary>
		/// <remarks>
		/// Adds a set of flags to the specified messages only if their mod-sequence value is less than the specified value.
		/// </remarks>
		/// <returns>The indexes of the messages that were not updated.</returns>
		/// <param name="indexes">The indexes of the messages.</param>
		/// <param name="modseq">The mod-sequence value.</param>
		/// <param name="flags">The message flags to add.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="indexes"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="indexes"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="indexes"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The <see cref="ImapFolder"/> does not support mod-sequences.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<IList<int>> AddFlagsAsync (IList<int> indexes, ulong modseq, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			return await AddFlagsAsync (indexes, modseq, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Removes a set of flags from the specified message.
		/// </summary>
		/// <remarks>
		/// Removes a set of flags from the specified message.
		/// </remarks>
		/// <param name="index">The index of the messages.</param>
		/// <param name="flags">The message flags to remove.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="index"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task RemoveFlagsAsync (int index, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await RemoveFlagsAsync (new [] { index }, flags, silent, cancellationToken);
		}

		/// <summary>
		/// Removes a set of flags from the specified message.
		/// </summary>
		/// <remarks>
		/// Removes a set of flags from the specified message.
		/// </remarks>
		/// <param name="index">The index of the messages.</param>
		/// <param name="flags">The message flags to remove.</param>
		/// <param name="userFlags">A set of user-defined flags to remove.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="index"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task RemoveFlagsAsync (int index, MessageFlags flags, HashSet<string> userFlags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await RemoveFlagsAsync (new [] { index }, flags, userFlags, silent, cancellationToken);
		}

		/// <summary>
		/// Removes a set of flags from the specified messages.
		/// </summary>
		/// <remarks>
		/// Removes a set of flags from the specified messages.
		/// </remarks>
		/// <param name="indexes">The indexes of the messages.</param>
		/// <param name="flags">The message flags to remove.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="indexes"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="indexes"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="indexes"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task RemoveFlagsAsync (IList<int> indexes, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await RemoveFlagsAsync (indexes, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Removes a set of flags from the specified messages only if their mod-sequence value is less than the specified value.
		/// </summary>
		/// <remarks>
		/// Removes a set of flags from the specified messages only if their mod-sequence value is less than the specified value.
		/// </remarks>
		/// <returns>The indexes of the messages that were not updated.</returns>
		/// <param name="indexes">The indexes of the messages.</param>
		/// <param name="modseq">The mod-sequence value.</param>
		/// <param name="flags">The message flags to remove.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="indexes"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="indexes"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="indexes"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The <see cref="ImapFolder"/> does not support mod-sequences.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<IList<int>> RemoveFlagsAsync (IList<int> indexes, ulong modseq, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			return await RemoveFlagsAsync (indexes, modseq, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Set the flags for the specified message.
		/// </summary>
		/// <remarks>
		/// Sets the flags for the specified message.
		/// </remarks>
		/// <param name="index">The index of the messages.</param>
		/// <param name="flags">The message flags to set.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="index"/> is invalid.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task SetFlagsAsync (int index, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await SetFlagsAsync (new [] { index }, flags, silent, cancellationToken);
		}

		/// <summary>
		/// Set the flags for the specified message.
		/// </summary>
		/// <remarks>
		/// Sets the flags for the specified message.
		/// </remarks>
		/// <param name="index">The index of the messages.</param>
		/// <param name="flags">The message flags to set.</param>
		/// <param name="userFlags">A set of user-defined flags to set.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="index"/> is invalid.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task SetFlagsAsync (int index, MessageFlags flags, HashSet<string> userFlags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await SetFlagsAsync (new [] { index }, flags, userFlags, silent, cancellationToken);
		}

		/// <summary>
		/// Set the flags for the specified messages.
		/// </summary>
		/// <remarks>
		/// Sets the flags for the specified messages.
		/// </remarks>
		/// <param name="indexes">The indexes of the messages.</param>
		/// <param name="flags">The message flags to set.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="indexes"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="indexes"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="indexes"/> is invalid.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task SetFlagsAsync (IList<int> indexes, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await SetFlagsAsync (indexes, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Sets the flags of the specified messages only if their mod-sequence value is less than the specified value.
		/// </summary>
		/// <remarks>
		/// Sets the flags of the specified messages only if their mod-sequence value is less than the specified value.
		/// </remarks>
		/// <returns>The indexes of the messages that were not updated.</returns>
		/// <param name="indexes">The indexes of the messages.</param>
		/// <param name="modseq">The mod-sequence value.</param>
		/// <param name="flags">The message flags to set.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageFlagsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="indexes"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="indexes"/> is empty.</para>
		/// <para>-or-</para>
		/// <para>One or more of the <paramref name="indexes"/> is invalid.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The <see cref="ImapFolder"/> does not support mod-sequences.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task<IList<int>> SetFlagsAsync (IList<int> indexes, ulong modseq, MessageFlags flags, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			return await SetFlagsAsync (indexes, modseq, flags, null, silent, cancellationToken);
		}

		/// <summary>
		/// Add a set of labels to the specified message.
		/// </summary>
		/// <remarks>
		/// Adds a set of labels to the specified message.
		/// </remarks>
		/// <param name="uid">The UID of the message.</param>
		/// <param name="labels">The labels to add.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageLabelsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uid"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task AddLabelsAsync (UniqueId uid, IList<string> labels, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await AddLabelsAsync (new [] { uid }, labels, silent, cancellationToken);
		}

		/// <summary>
		/// Remove a set of labels to the specified message.
		/// </summary>
		/// <remarks>
		/// Removes a set of labels to the specified message.
		/// </remarks>
		/// <param name="uid">The UID of the message.</param>
		/// <param name="labels">The labels to remove.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageLabelsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="uid"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task RemoveLabelsAsync (UniqueId uid, IList<string> labels, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await RemoveLabelsAsync (new [] { uid }, labels, silent, cancellationToken);
		}

		/// <summary>
		/// Set the labels of the specified message.
		/// </summary>
		/// <remarks>
		/// Sets the labels of the specified message.
		/// </remarks>
		/// <param name="uid">The UID of the message.</param>
		/// <param name="labels">The labels to set.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageLabelsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="uid"/> is invalid.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task SetLabelsAsync (UniqueId uid, IList<string> labels, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await SetLabelsAsync (new [] { uid }, labels, silent, cancellationToken);
		}

		/// <summary>
		/// Add a set of labels to the specified message.
		/// </summary>
		/// <remarks>
		/// Adds a set of labels to the specified message.
		/// </remarks>
		/// <param name="index">The index of the message.</param>
		/// <param name="labels">The labels to add.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageLabelsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="index"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task AddLabelsAsync (int index, IList<string> labels, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await AddLabelsAsync (new [] { index }, labels, silent, cancellationToken);
		}

		/// <summary>
		/// Remove a set of labels to the specified message.
		/// </summary>
		/// <remarks>
		/// Removes a set of labels to the specified message.
		/// </remarks>
		/// <param name="index">The index of the message.</param>
		/// <param name="labels">The labels to remove.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageLabelsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <para><paramref name="index"/> is invalid.</para>
		/// <para>-or-</para>
		/// <para>No flags were specified.</para>
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task RemoveLabelsAsync (int index, IList<string> labels, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await RemoveLabelsAsync (new [] { index }, labels, silent, cancellationToken);
		}

		/// <summary>
		/// Set the labels of the specified message.
		/// </summary>
		/// <remarks>
		/// Sets the labels of the specified message.
		/// </remarks>
		/// <param name="index">The index of the message.</param>
		/// <param name="labels">The labels to set.</param>
		/// <param name="silent">If set to <c>true</c>, no <see cref="MailFolder.MessageLabelsChanged"/> events will be emitted.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="index"/> is invalid.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="ImapClient"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="ImapClient"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// The <see cref="ImapClient"/> is not authenticated.
		/// </exception>
		/// <exception cref="FolderNotOpenException">
		/// The <see cref="ImapFolder"/> is not currently open in read-write mode.
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation was canceled via the cancellation token.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="ImapProtocolException">
		/// The server's response contained unexpected tokens.
		/// </exception>
		/// <exception cref="ImapCommandException">
		/// The server replied with a NO or BAD response.
		/// </exception>
		public async Task SetLabelsAsync (int index, IList<string> labels, bool silent, CancellationToken cancellationToken = default (CancellationToken))
		{
			await SetLabelsAsync (new [] { index }, labels, silent, cancellationToken);
		}
	}
}
