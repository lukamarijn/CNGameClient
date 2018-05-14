using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Form1 : Form
    {
        private string player1;
        private string currentPlayer;
        Button[] _buttonArray = new Button[9];
        TcpClient newClient;
        Thread t;
        private static string[] cell = new string[9];

        public Form1(string user)
        {
            InitializeComponent();
            _buttonArray = new Button[9] { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
            usernameBox.Text = user;
        

        }

        public void Connect()
        {

            if (usernameBox.Text.Length > 0)
            {
                usernameBox.Enabled = false;
                newClient = new TcpClient("localHost", 12345);

                t = new Thread(() => Listening());
                t.Start();
                string textToSend = "lo" + usernameBox.Text;
                byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);
                Stream newStream = newClient.GetStream();
                newStream.Write(bytesToSend, 0, bytesToSend.Length);
            }
        }

        private void Listening()
        {
            while (newClient.Connected)
            {
                NetworkStream nwStream = newClient.GetStream();
                byte[] buffer = new byte[newClient.ReceiveBufferSize];
                int bytesRead = nwStream.Read(buffer, 0, newClient.ReceiveBufferSize);
                string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                if (dataReceived != "")
                {
                    string receivedCommand = dataReceived.Substring(0, 2);
                    string receivedMessage = "";
                    if (dataReceived.Length > 2) receivedMessage = dataReceived.Remove(0, 2);
                    if (receivedCommand == "up")
                    {
                        if (listBox1.Enabled)
                        {
                            Invoke((MethodInvoker)(() => listBox1.Items.Clear()));
                            string[] tmp = receivedMessage.Split(',');
                            for (int i = 0; i < tmp.Length; i++)
                            {
                                if (tmp[i] != "")
                                {
                                    Invoke((MethodInvoker)(() => listBox1.Items.Add(tmp[i])));
                                }

                            }
                        }
                    }

                    if (receivedCommand == "st")
                    {
                        player1 = receivedMessage;
                       
                        MessageBox.Show(receivedMessage.ToString() + " wilt een game met jou spelen!");
                      
                       
                        byte[] bytesToSend = Encoding.UTF8.GetBytes("p1");
                        nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    }

                    if (receivedCommand == "gm")
                    {
                        for (int i = 0; i <_buttonArray.Length; i++)
                        {
                            Invoke((MethodInvoker)(() => _buttonArray[i].Enabled = true));
                        }

                        currentPlayer = receivedMessage;
                        foreach (var btn in _buttonArray)
                        {
                            if (btn.Text == "")

                                btn.Click += new System.EventHandler(this.ClickHandler);
                        }



                    }
                    if (receivedCommand == "br")
                    {
                        // refresh board

                        RefreshBoard(receivedMessage);
                        if (currentPlayer == player1)
                        {
                            byte[] bytesToSend = Encoding.UTF8.GetBytes("p2");
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        else
                        {
                            byte[] bytesToSend = Encoding.UTF8.GetBytes("p1");
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                    }

                }

            }
        }


        private void button10_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        int CheckStatus()
         {
             //CHECK X
             if (cell[0] == "X" && cell[3] == "X" && cell[5] == "X")
             {
                 return 1;
             }
             if (cell[3] == "X" && cell[4] == "X" && cell[5] == "X")
             {
                 return 1;
             }
             if (cell[2] == "X" && cell[5] == "X" && cell[8] == "X")
             {
                 return 1;
             }
             if (cell[1] == "X" && cell[4] == "X" && cell[7] == "X")
             {
                 return 1;
             }
             if (cell[0] == "X" && cell[1] == "X" && cell[2] == "X")
             {
                 return 1;
             }
             if (cell[6] == "X" && cell[7] == "X" && cell[8] == "X")
             {
                 return 1;
             }
             if (cell[0] == "X" && cell[4] == "X" && cell[8] == "X")
             {
                 return 1;
             }
             if (cell[2] == "X" && cell[4] == "X" && cell[6] == "X")
             {
                 return 1;
             }
            //CHECK 0
            if (cell[0] == "O" && cell[3] == "O" && cell[6] == "O")
            {
                return 2;
            }
            if (cell[3] == "O" && cell[4] == "O" && cell[5] == "O")
            {
                return 2;
            }
            if (cell[2] == "O" && cell[5] == "O" && cell[8] == "O")
            {
                return 2;
            }
            if (cell[1] == "O" && cell[4] == "O" && cell[7] == "O")
            {
                return 2;
            }
            if (cell[0] == "O" && cell[1] == "O" && cell[2] == "O")
            {
                return 2;
            }
            if (cell[6] == "O" && cell[7] == "O" && cell[8] == "O")
            {
                return 2;
            }
            if (cell[0] == "O" && cell[4] == "O" && cell[8] == "O")
            {
                return 2;
            }
            if (cell[2] == "O" && cell[4] == "O" && cell[6] == "O")
            {
                return 2;
            }

            //CHECK TIE
            int db = 0;
             for (int i = 0; i < 9; i++)
             {
                 if (cell[i] != "") db++;
             }
             if (db == 9)
             {
                 return 0;
             }

             return 3;

         }
       

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            Stream newStream = newClient.GetStream();
            string textToSend = "re";
            byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);
            newStream.Write(bytesToSend, 0, bytesToSend.Length);

        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            Stream newStream = newClient.GetStream();
            string textToSend = "pl";
            player1 = usernameBox.Text;

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Selecteer een tegenspeler");
            }
            if (listBox1.GetItemText(listBox1.SelectedItem) == usernameBox.Text)
            {
                Console.WriteLine(listBox1.GetItemText(listBox1.SelectedItem));
                MessageBox.Show("Je kan niet tegen jezelf spelen");
            }

            else
            {
                textToSend += listBox1.GetItemText(listBox1.SelectedItem) + "," + usernameBox.Text;
                byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);
                newStream.Write(bytesToSend, 0, bytesToSend.Length);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void ClickHandler(object sender, EventArgs e)
        {
            Button tempButton = (Button)sender;
            int index = _buttonArray.ToList().FindIndex(x => x.Name == tempButton.Name);

            if (currentPlayer == player1)
            {
                tempButton.Text = "X";

                cell[index] = "X";
                
            }
            else
            {
                tempButton.Text = "O";
                cell[index] = "O";
            }

            Stream newStream = newClient.GetStream();
            byte[] bytesToSend = Encoding.UTF8.GetBytes("sw" + ConvertStringArrayToString(cell));
            newStream.Write(bytesToSend, 0, bytesToSend.Length);

             for(int i = 0; i < _buttonArray.Length; i++)
            {
                Invoke((MethodInvoker)(() => _buttonArray[i].Enabled = false));
            }
            

           if (CheckStatus() == 1)
            {
                MessageBox.Show("Player1 Wins!");
                Application.Exit();
                
                
            }
           if (CheckStatus() == 2)
            {
                MessageBox.Show("Player2 Wins!");
                Application.Exit();
            }

           if (CheckStatus() == 3)
            {
                MessageBox.Show("No one wins...");
                Application.Exit();
            }
          
        }

       

        static string ConvertStringArrayToString(string[] array)
        {
         
            StringBuilder builder = new StringBuilder();
          
            foreach (string value in array)
            {
                

                if (value != null)
                {
                    builder.Append(value);
                }

                else
                {
                    builder.Append("-");
                }

                builder.Append('/');
                
            }
            return builder.ToString();
        }

        void RefreshBoard(string newValues)
        {
            Console.WriteLine(newValues);
            newValues = newValues.Substring(0, 17);

            Console.WriteLine(newValues);

            string[] boardValues = newValues.Split('/');

            int i = -1;

            foreach(var x in boardValues)
            {
                i = i + 1;
                if (x == "O")
                {

                    _buttonArray[i].Invoke((MethodInvoker)(() => _buttonArray[i].Text = "O"));
                }
                if (x == "X")
                {
                    _buttonArray[i].Invoke((MethodInvoker)(() => _buttonArray[i].Text = "X"));
                }
            }

            for (int y = 0; y < _buttonArray.Length; y++)
            {
                _buttonArray[y].Invoke((MethodInvoker)(() => _buttonArray[y].Enabled = true));
            }
            

            if (currentPlayer != player1)
            {
                currentPlayer = player1;
            }
            else
            {
                currentPlayer = "player2";
            }

           
        }

        
    }
}

