# OmniAdapter

## TODO

Move ResiliencePipeline Closer to the Client
merge the Azure ContentFilter into the FinishReason.ContentFilter

Clients:
- Audio
   - OpenAI - https://platform.openai.com/docs/api-reference/audio
     - Text To Speech
     - Speech to Text
   - ElevenLabs - https://elevenlabs.io/docs/api-reference/text-to-speech
     - Text To Speech
- Image
   - OpenAI - https://platform.openai.com/docs/api-reference/images/create
     - Generate Images
   - MidJourney - https://docs.midjourney.com/docs/quick-start
     - Generate Images
   - Leonardo - https://docs.leonardo.ai/docs/getting-started
     - Generate Images
     - Generate Motion
     - Generate Variations
     - Generate Textures
     - Meta Prompting

Interfaces:
- Competion (+Stream)
- Chat (+Stream)
- Audio (STT/TTS) (+Stream)
- Image

Kernel:
- Builder Pattern based on Interfaces and Configuration
- With Text/Audio/Image/...
- Tool Calls: Function Binding
- Function Binding via Registeration?
- Finish Reason: Length => Continue
- Polly: 429 Too Many Requests

Generator Project:
- Function Binding via Attrubute?
- JsonSchema as Input
- JsonNode as Output
