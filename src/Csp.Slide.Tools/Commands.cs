//#define ZH_Language
using Cocona;
using Slide;
using Slide.Common;
using Slide.Dicom;
using Slide.Sdpc;
using Slide.Sdpc.ColorControl;
using Slide.Sdpc.ColorControl.Adjust;
using Slide.Sdpc.ColorControl.Real;
using Sqray.Slide.Converter;
using Sqray.Slide.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csp.Slide.Tools
{
    internal sealed class Commands
    {
        private string _letterISOLanguageName = "en";
        public const int MaxParallel = 12;
        private readonly IConsoleMessage _consoleMessage;
        private readonly ConverterFactoryProvider _converterFactoryProvider;
        public Commands(IConsoleMessage consoleMessage, ConverterFactoryProvider provider)
        {
            _consoleMessage = consoleMessage;
            _converterFactoryProvider = provider;
            _letterISOLanguageName = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        }

        /// <summary>
        /// 批量格式转换命令行
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="dstPath"></param>
        /// <param name="format"></param>
        /// <param name="realCorrection"></param>
        /// <param name="parallelLevel"></param>
        /// <param name="desensitized"></param>
        /// <param name="hideThumbnail"></param>
        /// <returns></returns>
#if ZH_Language
        [Command("scvts", Description = "批量转换切片格式")]
        public async Task ConvertSlideBatchAsync(
            [Argument(Description = "待转换切片路径")] string srcPath,
            [Argument(Description = "目标保存路径")] string dstPath,
            [Option('f', Description = "显示指定目标格式的后缀，默认转成.svs")] string? format,
            [Option('r', Description = "开启真实校正")] bool? realCorrection,
            [Option('p', Description = "0-12的并行级别，越大并行程度越高，默认为12")] int? parallelLevel,
            [Option('d', Description = "脱敏操作")] bool? desensitized,
            [Option('s', Description = "隐藏缩略图")] bool? hideThumbnail,
            [Option('w', Description = "瓦片大小 - 宽")] int? tileWidth,
            [Option('h', Description = "瓦片大小 - 高")] int? tileHeight,
            [Option('q', Description = "压缩质量")] int? quality,
            [Option('e', Description = "导出指定层级JPEG")] int? exportLayer)
#else
        [Command("scvts", Description = TextConstant.ConvertSlideBatchDescription)]
        public async Task ConvertSlideBatchAsync(
            [Argument(Description = TextConstant.SrcPathDescription)] string srcPath,
            [Argument(Description = TextConstant.DstPathDescription)] string dstPath,
            [Option('f', Description = TextConstant.FormatDescription)] string? format,
            [Option('r', Description = TextConstant.RealCorrectionDescription)] bool? realCorrection,
            [Option('p', Description = TextConstant.ParallelLevelDescription)] int? parallelLevel,
            [Option('d', Description = TextConstant.DesensitizedDescription)] bool? desensitized,
            [Option('s', Description = TextConstant.HideThumbnailDescription)] bool? hideThumbnail,
            [Option('w', Description = TextConstant.TileWidthDescription)] int? tileWidth,
            [Option('h', Description = TextConstant.TileHeightDescription)] int? tileHeight,
            [Option('q', Description = TextConstant.QualityDescription)] int? quality,
            [Option('e', Description = TextConstant.ExportLayerDescription)] int? exportLayer)
#endif
        {
            Debug.Assert(_converterFactoryProvider != null);
            Debug.Assert(srcPath != null);
            Debug.Assert(dstPath != null);
            if (srcPath.ArePathsSame(dstPath)) { _consoleMessage.WriteLine(GetLanguageText(nameof(TextConstant.ArePathsSameDescription))/*"目标保存路径 不能与 待转换切片路径相同"*/); return; }
            if (string.IsNullOrWhiteSpace(format)) format=".svs";

            _consoleMessage.WriteLine(_letterISOLanguageName);

            if (srcPath.IsFolder())
            {
                if (!Directory.Exists(dstPath)) Directory.CreateDirectory(dstPath);

                var files = srcPath.RenameSameNameFiles();

                //_consoleMessage.WriteLine($"ConvertSlideBatchAsync started on thread:{Environment.CurrentManagedThreadId}");

                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };

                var finish = 0;
                foreach (var file in files)
                {
                    //_consoleMessage.WriteLine($"正在转换-> {file.Item1} ");
                    _consoleMessage.WriteLine($"{GetLanguageText(nameof(TextConstant.ConvertingDescription))} {file.Item1} ");
                    var destPath = $"{Path.Combine(dstPath, Path.GetFileNameWithoutExtension(file.Item2))}{format}";
                    ConvertSlide(file.Item1, destPath, format, realCorrection, parallelLevel, desensitized, hideThumbnail, tileWidth, tileHeight, quality, exportLayer);
                    //_consoleMessage.WriteLine($"已转换 {++finish} 个/共{files.Count()}个");
                    _consoleMessage.WriteLine($"{GetLanguageText(nameof(TextConstant.FinishConvertDescription))} {++finish} {GetLanguageText(nameof(TextConstant.PiecesDescription))}/{GetLanguageText(nameof(TextConstant.TotalDescription))} {files.Count()} {GetLanguageText(nameof(TextConstant.PiecesDescription))}");
                    _consoleMessage.WriteLine("");
                }

                //_consoleMessage.WriteLine($"ConvertSlideBatchAsync completed on thread:{ Environment.CurrentManagedThreadId}");
            }
            else
                ConvertSlide(srcPath, dstPath, format, realCorrection, parallelLevel, desensitized, hideThumbnail, tileWidth, tileHeight, quality, exportLayer);
        }


        /// <summary>
        /// 格式转换命令行
        /// </summary>
        /// <param name="srcPath">待转换切片路径</param>
        /// <param name="dstPath">目标保存路径</param>
        /// <param name="format">显示指定目标格式的后缀</param>
        /// <param name="realCorrection">开启真实校正</param>
        /// <param name="parallelLevel">0-12的并行级别，越大并行程度越高，默认为12</param>
        /// <param name="desensitized">脱敏操作</param>
        /// <param name="hideThumbnail">隐藏缩略图</param>
#if ZH_Language
        [Command("scvt", Description = "转换切片格式")]
        public void ConvertSlide(
            [Argument(Description = "待转换切片路径")] string srcPath,
            [Argument(Description = "目标保存路径")] string dstPath,
            [Option('f', Description = "显示指定目标格式的后缀，如.sdpc")] string? format,
            [Option('r', Description = "开启真实校正")] bool? realCorrection,
            [Option('p', Description = "0-12的并行级别，越大并行程度越高，默认为12")] int? parallelLevel,
            [Option('d', Description = "脱敏操作")] bool? desensitized,
            [Option('s', Description = "隐藏缩略图")] bool? hideThumbnail,
            [Option('w', Description = "瓦片大小 - 宽")] int? tileWidth,
            [Option('h', Description = "瓦片大小 - 高")] int? tileHeight,
            [Option('q', Description = "压缩质量")] int? quality,
            [Option('e', Description = "导出指定层级JPEG")] int? exportLayer)
#else
        [Command("scvt", Description = TextConstant.ConvertSlideDescription)]
        public void ConvertSlide(
            [Argument(Description = TextConstant.SrcPathDescription)] string srcPath,
            [Argument(Description = TextConstant.DstPathDescription)] string dstPath,
            [Option('f', Description = TextConstant.FormatDescription)] string? format,
            [Option('r', Description = TextConstant.RealCorrectionDescription)] bool? realCorrection,
            [Option('p', Description = TextConstant.ParallelLevelDescription)] int? parallelLevel,
            [Option('d', Description = TextConstant.DesensitizedDescription)] bool? desensitized,
            [Option('s', Description = TextConstant.HideThumbnailDescription)] bool? hideThumbnail,
            [Option('w', Description = TextConstant.TileWidthDescription)] int? tileWidth,
            [Option('h', Description = TextConstant.TileHeightDescription)] int? tileHeight,
            [Option('q', Description = TextConstant.QualityDescription)] int? quality,
            [Option('e', Description = TextConstant.ExportLayerDescription)] int? exportLayer)
#endif
        {
            Debug.Assert(_converterFactoryProvider != null);
            Debug.Assert(srcPath != null);
            Debug.Assert(dstPath != null);
            if (tileWidth.HasValue && tileWidth.Value<=0) { Console.WriteLine(GetLanguageText(nameof(TextConstant.TileWidthLessThanDescription))/*"瓦片大小宽 -w 参数值 不能小于0"*/); return; }
            if (tileHeight.HasValue && tileHeight.Value<=0) { Console.WriteLine(GetLanguageText(nameof(TextConstant.TileHeightLessThanDescription))/*"瓦片大小高 -h 参数值 不能小于0"*/); return; }

            var cvtOption = new ConversionOptions()
            {
                TargetFormat = GetTargetFormat(format, dstPath),
                IsDesensitized = desensitized ?? false,
                IsShowThumbnail = !hideThumbnail ?? true,
                TileSize = tileWidth.HasValue && tileHeight.HasValue ? new System.Drawing.Size(tileWidth.Value, tileHeight.Value) : null,
                Quality = quality,
                ExportLayerJPEG = exportLayer
            };

            IConverterFactory factory;
            try
            {
                factory = _converterFactoryProvider.GetConverterFactory(cvtOption);
            }
            catch (InvalidOperationException)
            {
                //_consoleMessage.WriteLine($"不支持的目标格式:{cvtOption.TargetFormat}");
                _consoleMessage.WriteLine($"{GetLanguageText(nameof(TextConstant.NotSupportTargetFormatDescription))}:{cvtOption.TargetFormat}");
                return;
            }
            using SlideImage? srcSlide = LoadImage.LoadObjectImage(srcPath) as SlideImage;
            if (srcSlide == null)
            {
                //_consoleMessage.WriteLine($"{srcPath}解析失败。");
                _consoleMessage.WriteLine($"{srcPath} {GetLanguageText(nameof(TextConstant.ConvertFailedDescription))}");
                return;
            }
            if (realCorrection == true)
                SetColorStyle(srcSlide);
            //_consoleMessage.WriteLine($"转换开始");
            IDataTargetOwner dataTargetOwner = new DataTargetOwner(dstPath);
            using IConverter converter = factory.CreateConverter(srcSlide, dataTargetOwner, cvtOption);
            converter.ParallelLevel = GetParallelLevel(parallelLevel ?? MaxParallel);
            var curProgress = 0;
            converter.ProgressAction += progress =>
            {
                if (progress == curProgress)
                    return;
                curProgress = progress;
                _consoleMessage.Write($"progress:{curProgress}");
            };
            var sw = Stopwatch.StartNew();
            converter.Convert(CancellationToken.None);
            //_consoleMessage.WriteLine($"转换成功,耗时{sw.Elapsed.TotalSeconds}秒");
            _consoleMessage.WriteLine($"{GetLanguageText(nameof(TextConstant.ConvertSucceededDescription))}{sw.Elapsed.TotalSeconds} s");
        }

        /// <summary>
        /// 设置真实颜色校正
        /// </summary>
        /// <param name="src"></param>
        private void SetColorStyle(SlideImage src)
        {
            if(src is SdpcImage sdpcImage)
            {
                sdpcImage.InitColorTable();
                sdpcImage.InitTileImage();
                var colorTable = sdpcImage.Sp.ColorTable;
                var colorStyle = new RealStyle(colorTable);
                colorStyle.RefreshAdjustParm(new ColorAdjustParam());
                //sdpcImage.SetColorStyle = colorStyle.SetArgbColorStyle;
                sdpcImage.ColorStyle = colorStyle;
            }
            else if(src is DcmImage dcmImage)
            {
                dcmImage.InitColorTable();
                var colorTable = dcmImage.ColorTable;
                var colorStyle = new RealStyle(colorTable);
                colorStyle.RefreshAdjustParm(new ColorAdjustParam());
                //dcmImage.SetColorStyle = colorStyle.SetArgbColorStyle;
                dcmImage.ColorStyle = colorStyle;
            }
        }

        /// <summary>
        /// 获取目标转换格式
        /// </summary>
        /// <param name="format">可能存在的格式后缀名</param>
        /// <param name="dstPath">目标保存路径</param>
        /// <returns></returns>
        private string GetTargetFormat(string? format, string dstPath)
        {
            if (format != null)
                return format;
            var suffix = Path.GetExtension(dstPath);
            if (string.IsNullOrEmpty(suffix))
                return ".dcm";
            return suffix;
        }

        /// <summary>
        /// 获取并发水平
        /// </summary>
        /// <param name="level">0-12的并行级别，越大并行程度越高，默认为12</param>
        /// <returns></returns>
        private ParallelLevel GetParallelLevel(int level)
        {
            var levelArr = Enum.GetValues(typeof(ParallelLevel)).Cast<ParallelLevel>().ToArray();
            var index = (int)(1.0 * level / MaxParallel * levelArr.Length);
            if (index == levelArr.Length)
                --index;
            return levelArr[index];
        }

        private string GetLanguageText(string propertyName)
        {
            if (_letterISOLanguageName=="zh") return typeof(TextConstant).GetField(propertyName)?.GetDescription() ?? string.Empty;
            return typeof(TextConstant).GetField(propertyName)?.GetRawConstantValue()?.ToString() ?? string.Empty;
        }
    }
}
