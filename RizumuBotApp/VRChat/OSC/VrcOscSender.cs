using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using OSCforPCL;
using RizumuBot;

namespace RizumuBot.VRChat.OSC
{
    public class VrcOscSender : IDisposable
    {
        private readonly ILog logger;
        private OSCClient oscClient;

        /// <summary>
        /// Initializes to OSCClient that will speak to vrc
        /// </summary>
        /// <param name="logger"></param>
        public VrcOscSender(ILog logger)
        {
            oscClient = new OSCClient(Config.OscServer, Config.OscPort);

            oscClient.OnReplyReceived += OscClient_OnReplyReceived;
            this.logger = logger;
        }

        /// <summary>
        /// Logs responses from vrc for debugging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OscClient_OnReplyReceived(object sender, OSCMessageReceivedArgs e)
        {
            logger.Debug($"{nameof(VrcOscSender)}.{nameof(this.OscClient_OnReplyReceived)}: {e.Message.GetByteLength()} byte(s). Arguments: {String.Join(',', e.Message.Arguments.Select(a => a.TypeTag))}");
        }

        /// <summary>
        /// Sends a command to vrc
        /// </summary>
        /// <param name="command"></param>
        public async System.Threading.Tasks.Task SendMessageAsync(VrcCameraCommands command)
        {
            if (command == VrcCameraCommands.None) return;

            async Task sendMessage(float value)
            {
                OSCMessage message = new OSCMessage($"/riz/{command.ToString().ToLowerInvariant()}", value);
                logger.Debug($"{nameof(VrcOscSender)}.{nameof(this.SendMessageAsync)}: {nameof(command)} {command}. Message: {Encoding.UTF8.GetString(message.Bytes)}");
                await oscClient.Send(message);
            }

            
            await sendMessage(1.0f); //on
            await Task.Delay(1000);
            await sendMessage(0.0f); //off
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    oscClient = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OSCTest() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
