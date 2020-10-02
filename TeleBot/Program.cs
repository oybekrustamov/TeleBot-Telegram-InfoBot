using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.IO;

namespace TeleBot
{
    class Program
    {
        private static ITelegramBotClient client;
        static int index =0;
        public struct Messages {
            public string cmd;
            public string msg;
        };
        public static Messages[] cmds = new Messages[100000000];
        static void Main(string[] args)
        {
            Console.WriteLine("Telegram Bot has been started.");
            Read_CMDS();

            client = new TelegramBotClient("1371055202:AAH4BC7F-Youd6rI7MLQP0kg5UvgPYevprg")
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            var request = client.GetMeAsync().Result;
            Console.WriteLine(request.Id + " ~ " + request.FirstName);


            client.OnMessage += Client_OnMessage;
            client.StartReceiving();
            Console.WriteLine("Don't press any key in Console! Otherwise you need run this app again!");
            Console.ReadKey();
        }

        private static async void Client_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            foreach(Messages ms in cmds)
            {
                if (ms.cmd == e.Message.Text)
                {
                    await client.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: ms.msg
                        ).ConfigureAwait(false);
                    break;
                }
            }
        }

        private static void Read_CMDS()
        {

            try
            {
                StreamReader cmd_file = new StreamReader("./data/cmd.txt");
                StreamReader msg_file = new StreamReader("./data/msg.txt");
                while (!cmd_file.EndOfStream)
                {
                    cmds[index].cmd = cmd_file.ReadLine();
                    cmds[index].msg = msg_file.ReadLine();
                    index++;
                }
                cmd_file.Close();
                msg_file.Close();
            }
            catch(Exception errormsg)
            {
                Console.WriteLine(errormsg.Message);
            }
        }
    }
}
