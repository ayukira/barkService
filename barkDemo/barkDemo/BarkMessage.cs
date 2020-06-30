using System;
using System.Collections.Generic;
using System.Text;

namespace barkDemo
{
    public class BarkMessage
    {
        public BarkMessage(string body) : this(string.Empty, body) { }
        public BarkMessage(string title, string body)
        {
            this.title = title;
            this.body = body;
        }

        #region 公共属性
        /// <summary>
        /// 标题,加粗
        /// </summary>
        public string title { get; set; } = string.Empty;
        /// <summary>
        /// 正文
        /// </summary>
        public string body { get; set; } = string.Empty;
        /// <summary>
        /// 自动保存
        /// </summary>
        public string isArchive { get; private set; } = "1";
        /// <summary>
        /// 链接
        /// </summary>
        public string url { get; private set; } = string.Empty;
        /// <summary>
        /// 自动复制
        /// </summary>
        public string autoMaticallyCopy { get; private set; } = "0";
        /// <summary>
        /// 复制文本
        /// </summary>
        public string copy { get; private set; } = string.Empty;
        #endregion

        #region 公共方法
        /// <summary>
        /// 设置链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public BarkMessage SetUrl(string url)
        {
            this.url = url;
            return this;
        }
        /// <summary>
        /// 设置保存，默认保存
        /// </summary>
        /// <returns></returns>
        public BarkMessage SetArchive()
        {
            isArchive = "1";
            return this;
        }
        /// <summary>
        /// 设置不保存，默认保存
        /// </summary>
        /// <returns></returns>
        public BarkMessage SetNotArchive()
        {
            isArchive = "0";
            return this;
        }
        /// <summary>
        /// 设置自动复制，默认不自动复制
        /// </summary>
        /// <returns></returns>
        public BarkMessage SetAutoCopy()
        {
            this.autoMaticallyCopy = "1";
            return this;
        }
        /// <summary>
        /// 设置不自动复制，默认不自动复制
        /// </summary>
        /// <returns></returns>
        public BarkMessage SetNotAutoCopy()
        {
            this.autoMaticallyCopy = "1";
            return this;
        }
        /// <summary>
        /// 设置自动拷贝的文本，默认拷贝全文
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public BarkMessage SetCopyText(string text)
        {
            copy = text;
            return this;
        }
        #endregion
    }
}
