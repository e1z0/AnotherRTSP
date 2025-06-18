/*
 * Copyright (c) 2024-2025 e1z0. All Rights Reserved.
 * Licensed under the Business Source License 1.1.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace AnotherRTSP.Components
{
    public class TransparentLabel : Label
    {

        public TransparentLabel() : base()
        {
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                    return cp;
            }
        }
        protected override void WndProc(ref Message m)
        {
                if (m.Msg != 0x14 /*WM_ERASEBKGND*/ && m.Msg != 0x0F /*WM_PAINT*/)
                    base.WndProc(ref m);
                else
                {
                    if (m.Msg == 0x0F) // WM_PAINT
                        base.OnPaint(new PaintEventArgs(Graphics.FromHwnd(Handle), ClientRectangle));
                    DefWndProc(ref m);
                }
           
        }
    }

}
