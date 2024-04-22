using Boundless.OmniAdapter.Interfaces;
using System.Runtime.CompilerServices;

namespace Boundless.OmniAdapter.Huggingface;

//public class HuggingfaceClient : IChatCompetition, IChatStream, ITextCompetition
//{
//  /// <summary>
//  /// Creates a completion for the chat message.
//  /// </summary>
//  /// <param name="chatRequest">The chat request which contains the message content.</param>
//  /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
//  /// <returns><see cref="ChatResponse"/>.</returns>
//  public async Task<ChatResponse> GetChatCompletionAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default)
//  {
//    throw new NotImplementedException();
//  }

//  /// <summary>
//  /// Created a completion for the chat message and stream the results as they come in.<br/>
//  /// </summary>
//  /// <param name="chatRequest">The chat request which contains the message content.</param>
//  /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
//  /// <returns><see cref="ChatResponse"/>.</returns>
//  public async IAsyncEnumerable<ChatResponse> StreamChatCompletionAsync(ChatRequest chatRequest, [EnumeratorCancellation] CancellationToken cancellationToken = default)
//  {
//    throw new NotImplementedException();
//  }

//  public Task<TextResponse> GetTextCompletionAsync(TextRequest chatRequest, CancellationToken cancellationToken = default)
//  {
//  }
//}
