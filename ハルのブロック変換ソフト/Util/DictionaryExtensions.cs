using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ハルのブロック変換ソフト.Util
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 値を取得、keyがなければデフォルト値を設定し、デフォルト値を取得
        /// </summary>
        public static TValue TryGetValueEx<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {

            //Dictionary自体がnullの場合はインスタンス作成
            if (source == null)
            {
                source = new Dictionary<TKey, TValue>();
            }

            //keyが存在しない場合はデフォルト値を設定
            if (!source.ContainsKey(key))
            {
                source[key] = defaultValue;
            }

            return source[key];
        }
    }
}
