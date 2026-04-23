using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csp.Slide.Tools
{
    public static class TextConstant
    {
        [Description("批量转换切片格式")]public const string ConvertSlideBatchDescription = "Batch convert slide format";
        [Description("转换切片格式")] public const string ConvertSlideDescription = "convert slide format";

        [Description("待转换切片路径")] public const string SrcPathDescription = "Path of slides to be converted";
        [Description("目标保存路径")] public const string DstPathDescription = "Target save path";
        [Description("显示指定目标格式的后缀，默认转成.svs")] public const string FormatDescription = "Show the suffix of the specified target format, converted to .svs by default";
        [Description("开启真实校正")] public const string RealCorrectionDescription = "Turn on true correction";
        [Description("0-12的并行级别，越大并行程度越高，默认为12")] public const string ParallelLevelDescription = "Parallel level 0-12, the larger the level, the higher the parallelism, the default is 12";
        [Description("脱敏操作")] public const string DesensitizedDescription = "Desensitization operation";
        [Description("隐藏缩略图")] public const string HideThumbnailDescription = "Hide thumbnail";
        [Description("瓦片大小 - 宽")] public const string TileWidthDescription = "Tile size - width";
        [Description("瓦片大小 - 高")] public const string TileHeightDescription = "Tile size - height";
        [Description("压缩质量")] public const string QualityDescription = "Compression quality";
        [Description("导出指定层级JPEG")] public const string ExportLayerDescription = "Export Layer JPEG  Slide Image";

        [Description("目标保存路径 不能与 待转换切片路径相同")] public const string ArePathsSameDescription = "Target save path cannot be the same as the path of slices to be converted";
        [Description("正在转换-> ")] public const string ConvertingDescription = "Converting-> ";
        [Description("已转换 ")] public const string FinishConvertDescription = "Converted ";
        [Description("共")] public const string TotalDescription = "total";
        [Description("个")] public const string PiecesDescription = "pieces";

        [Description("瓦片大小宽 -w 参数值 不能小于0")] public const string TileWidthLessThanDescription = "Tile size width -w parameter value cannot be less than 0";
        [Description("瓦片大小高 -h 参数值 不能小于0")] public const string TileHeightLessThanDescription = "Tile size height -h parameter value cannot be less than 0";
        [Description("不支持的目标格式")] public const string NotSupportTargetFormatDescription = "Unsupported target format";
        [Description("解析失败。")] public const string ConvertFailedDescription = "Convert Failed";
        [Description("转换成功,耗时")] public const string ConvertSucceededDescription = "Conversion successful, took ";

        [Description("路径是文件夹。")] public const string PathFolderDescription = "Path is a folder";
        [Description("路径是文件。")] public const string PathFileDescription = "Path is a file";
        [Description("路径不存在。")] public const string PathNotExistDescription = "Path does not exist";
    }
}
