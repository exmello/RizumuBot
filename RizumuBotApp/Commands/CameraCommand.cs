using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RizumuBot.Data;
using RizumuBot.Model;
//using RizumuBot.TwitchApi;
using RizumuBot.VRChat.OSC;

namespace RizumuBot.Commands
{
    /// <summary>
    /// Responds to !viewer with how many viewers have watched the current stream from beginning to end
    /// </summary>
    public class CameraCommand : ICommand
    {
        private readonly Regex regCameraCommand;
        private readonly TwitchResponseWriter tw;
        private readonly VrcOscSender osc;
        //private readonly IDictionaryRepository repo;

        public CameraCommand(TwitchResponseWriter tw, VrcOscSender osc)//, IDictionaryRepository repo)
        {
            this.tw = tw;
            this.osc = osc;
            //this.repo = repo;
            this.regCameraCommand = new Regex("^!camera\\s(?<param>.*?)\\s$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        public bool IsMatch(MessageInfo message)
        {
            return regCameraCommand.IsMatch(message.Content);
        }

        async Task ICommand.Process(MessageInfo message)
        {
            //get template
            string msgParam;
            Match match = regCameraCommand.Match(message.Content);
            if (match.Success)
            {
                if (!string.IsNullOrWhiteSpace(match.Groups["param"].Value))
                {
                    msgParam = match.Groups["param"].Value.ToLowerInvariant().Trim();

                    string response = string.Empty;
                    VrcCameraCommands command;

                    // Choose response and osc command based on !camera <param> value
                    switch (msgParam)
                    {
                        case "stairs":
                        case "back":
                            response = "Moving camera to the back.";
                            command = VrcCameraCommands.Back;
                            break;
                        case "dj":
                        case "front":
                            response = "Moving camera to the DJ.";
                            command = VrcCameraCommands.DJ;
                            break;
                        case "stage":
                        case "side":
                            response = "Moving camera to the stage.";
                            command = VrcCameraCommands.Stage;
                            break;
                        case "move":
                        case "animate":
                            response = "Animating camera.";
                            command = VrcCameraCommands.Move;
                            break;
                        default:
                            response = $"Follow !camera with one of the following parameters: {GetCameraCommands()}";
                            command = VrcCameraCommands.None;
                            break;
                    }

                    await tw.RespondMessageAsync(response);    // Respond to twitch chat
                    await osc.SendMessageAsync(command);       // Send OSC command to vrc
                }
                else
                {
                    await tw.RespondMessageAsync($"Follow !camera with one of the following parameters: {GetCameraCommands()}");
                }
            }

        }

        private string GetCameraCommands()
        {
            return String.Join(' ', Enum.GetNames(typeof(VrcCameraCommands)).Where(n => n != "None").Select(n => n.ToLowerInvariant()));
        }
    }
}
