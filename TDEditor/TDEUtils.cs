#region
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TimeDRODPOF.TDStructure;

#endregion

namespace TimeDRODPOF.TDEditor
{
    public static class TDEUtils
    {
        public static TDSRoom InputCreateRoomBox(TDSRoom currentRoom)
        {
            var form = new Form();
            var westRadioButton = new RadioButton {Text = "To the west", Enabled = false};
            var eastRadioButton = new RadioButton {Text = "To the east", Enabled = false};
            var northRadioButton = new RadioButton {Text = "To the north", Enabled = false};
            var southRadioButton = new RadioButton {Text = "To the south", Enabled = false};
            var isRequiredCheckBox = new CheckBox {Text = "Is required", Checked = true};
            var isSecretCheckBox = new CheckBox {Text = "Is secret", Checked = false};
            var doneButton = new Button {Text = "OK"};

            form.Text = "Create new room";

            form.ClientSize = new Size(396, 200);
            form.Controls.AddRange(new Control[]
                                   {
                                       westRadioButton, eastRadioButton, northRadioButton, southRadioButton,
                                       isRequiredCheckBox, isSecretCheckBox, doneButton
                                   });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;

            westRadioButton.SetBounds(25, 25, 100, 25);
            eastRadioButton.SetBounds(25, 50, 100, 25);
            northRadioButton.SetBounds(25, 75, 100, 25);
            southRadioButton.SetBounds(25, 100, 100, 25);
            isRequiredCheckBox.SetBounds(125, 25, 100, 25);
            isSecretCheckBox.SetBounds(125, 50, 100, 25);
            doneButton.SetBounds(125, 100, 100, 25);

            if (currentRoom.Level.GetRoom(currentRoom.X - 1, currentRoom.Y) == null) westRadioButton.Enabled = true;
            if (currentRoom.Level.GetRoom(currentRoom.X + 1, currentRoom.Y) == null) eastRadioButton.Enabled = true;
            if (currentRoom.Level.GetRoom(currentRoom.X, currentRoom.Y - 1) == null) northRadioButton.Enabled = true;
            if (currentRoom.Level.GetRoom(currentRoom.X, currentRoom.Y + 1) == null) southRadioButton.Enabled = true;

            TDSRoom result = null;
            var resultX = 0;
            var resultY = 0;

            doneButton.Click += (e, sender) =>
                                {
                                    if (westRadioButton.Checked) resultX = -1;
                                    if (eastRadioButton.Checked) resultX = 1;
                                    if (northRadioButton.Checked) resultY = -1;
                                    if (southRadioButton.Checked) resultY = 1;

                                    result = TDSControl.CreateRoom(currentRoom.Level, currentRoom.X + resultX, currentRoom.Y + resultY, isRequiredCheckBox.Checked, isSecretCheckBox.Checked);
                                    form.Close();
                                };

            form.ShowDialog();

            return result;
        }

        public static string InputStringBox(string mTitle, string mDefaultText = "", string mNullText = "")
        {
            var form = new Form();
            var textBox = new TextBox();

            form.Text = mTitle;
            textBox.Text = mDefaultText;
            textBox.SetBounds(12, 100, 372, 20);

            form.ClientSize = new Size(396, 200);
            form.Controls.AddRange(new Control[] {textBox});
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            textBox.KeyDown += (sender, args) =>
                               {
                                   if (args.KeyCode == Keys.Enter)
                                       form.Close();
                               };

            form.ShowDialog();

            return string.IsNullOrEmpty(textBox.Text) ? mNullText : textBox.Text;
        }

        public static int InputListBox(string mTitle, IEnumerable<string> mItems)
        {
            var form = new Form();
            var listBox = new ListBox();

            form.Text = mTitle;

            foreach (var item in mItems)
                listBox.Items.Add(item);

            listBox.SetBounds(9, 20, 372, 250);
            listBox.AutoSize = true;

            form.ClientSize = new Size(396, 300);
            form.Controls.AddRange(new Control[] {listBox});
            form.ClientSize = new Size(Math.Max(300, listBox.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            listBox.DoubleClick += (sender, args) => { if (listBox.SelectedIndex != -1) form.Close(); };

            form.ShowDialog();

            return listBox.SelectedIndex;
        }

        public static void InputParametersBox(string title, TDEEntity mEntity)
        {
            var form = new Form();
            var listBox = new ListBox();
            var buttonExit = new Button();
            var textBox = new TextBox();

            form.Text = title;

            foreach (var parameter in mEntity.Parameters)
                listBox.Items.Add(String.Format("Parameter {0}: {1}", parameter.Name, parameter));

            buttonExit.Text = "Finished";
            buttonExit.DialogResult = DialogResult.Cancel;

            textBox.SetBounds(12, 280, 372, 20);
            listBox.SetBounds(9, 20, 372, 250);
            buttonExit.SetBounds(309, 320, 75, 23);

            listBox.AutoSize = true;
            buttonExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 615);
            form.Controls.AddRange(new Control[] {listBox, textBox, buttonExit});
            form.ClientSize = new Size(Math.Max(300, listBox.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            listBox.Click += (o, args) =>
                             {
                                 InputParametersBoxChange(mEntity, listBox, textBox.Text);
                                 textBox.Focus();
                             };
            form.CancelButton = buttonExit;

            form.ShowDialog();
        }

        private static void InputParametersBoxChange(TDEEntity mEntity, ListBox mListBox, string mValue)
        {
            var index = mListBox.SelectedIndex;

            if (index != -1)
            {
                var parameter = mEntity.Parameters[index];

                if (parameter.TypeString == "int")
                {
                    int resultValue;
                    var success = int.TryParse(mValue, out resultValue);

                    if (!success) return;

                    parameter.Value = resultValue;
                }
                else if (parameter.TypeString == "bool")
                {
                    bool resultValue;
                    var success = bool.TryParse(mValue, out resultValue);

                    if (!success) return;

                    parameter.Value = resultValue;
                }
                else if (parameter.TypeString == "list<int>")
                {
                    var resultValue = new List<int>();
                    var split = mValue.Split(',');

                    foreach (var splitInt in split)
                    {
                        int splitIntTemp;
                        var success = int.TryParse(splitInt, out splitIntTemp);
                        if (success) resultValue.Add(splitIntTemp);
                    }

                    parameter.Value = resultValue;
                }
                else if (parameter.TypeString == "string")
                {
                    var resultValue = mValue;
                    parameter.Value = resultValue;
                }
            }

            for (var i = 0; i < mEntity.Parameters.Count; i++)
            {
                var parameter = mEntity.Parameters[i];
                mListBox.Items[i] = (String.Format("Parameter {0}: {1}", parameter.Name, parameter));
            }

            mListBox.SelectedIndex = -1;
        }
    }
}