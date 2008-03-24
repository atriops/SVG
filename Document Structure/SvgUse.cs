using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing.Drawing2D;

namespace Svg
{
    public class SvgUse : SvgGraphicsElement
    {
        private Uri _referencedElement;

        [SvgAttribute("href")]
        public virtual Uri ReferencedElement
        {
            get { return this._referencedElement; }
            set { this._referencedElement = value; }
        }

        [SvgAttribute("x")]
        public virtual SvgUnit X
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("x"); }
            set { this.Attributes["x"] = value; }
        }

        [SvgAttribute("y")]
        public virtual SvgUnit Y
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("y"); }
            set { this.Attributes["y"] = value; }
        }

        protected internal override void PushTransforms(System.Drawing.Graphics graphics)
        {
            base.PushTransforms(graphics);
            graphics.TranslateTransform(this.X.ToDeviceValue(this), this.Y.ToDeviceValue(this, true));
        }

        public SvgUse()
        {
            
        }

        public override System.Drawing.Drawing2D.GraphicsPath Path
        {
            get { return null; }
        }

        public override System.Drawing.RectangleF Bounds
        {
            get { return new System.Drawing.RectangleF(); }
        }

        protected override string  ElementName
        {
            get { return "use"; }
        }

        protected override void Render(System.Drawing.Graphics graphics)
        {
            this.PushTransforms(graphics);

            SvgGraphicsElement element = (SvgGraphicsElement)this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement);
            // For the time of rendering we want the referenced element to inherit
            // this elements transforms
            SvgElement parent = element._parent;
            element._parent = this;
            element.RenderElement(graphics);
            element._parent = parent;

            this.PopTransforms(graphics);
        }
    }
}