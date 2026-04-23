用法

■ Windows
1.批量转换命令执行示例  .\sstools.exe scvts C:\srcFolder C:\destFolder
2.开启颜色校正 通过添加参数 -r：   .\sstools.exe scvts C:\srcFolder C:\destFolder -r
3.指定转换目标格式 通过添加参数 -f：  .\sstools.exe scvts C:\srcFolder C:\destFolder -f 目标格式
示例
同时开启颜色校正和指定转换目标格式 .sdpc
.\sstools.exe scvts C:\srcFolder C:\destFolder -r -f .sdpc

■ Linux / macOS
1.批量转换命令执行示例  ./sstools scvts /path/srcFolder /path/destFolder
2.开启颜色校正 通过添加参数 -r：   ./sstools scvts /path/srcFolder /path/destFolder -r
3.指定转换目标格式 通过添加参数 -f：  ./sstools scvts /path/srcFolder /path/destFolder -f 目标格式
示例
同时开启颜色校正和指定转换目标格式 .sdpc
./sstools scvts /path/srcFolder /path/destFolder -r -f .sdpc

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
平台初始化（仅需执行一次）
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

■ Linux x64（Ubuntu/Debian，x86_64）
  执行：sudo bash run.sh
  说明：将当前目录写入 LD_LIBRARY_PATH。

■ Linux ARM64（Ubuntu/Debian，AArch64，如 AWS Graviton、树莓派 64 位）
  执行：sudo bash run.sh
  说明：将当前目录写入 LD_LIBRARY_PATH。

■ macOS ARM64（Apple Silicon，M 系列芯片）
  前提：已安装 Homebrew（https://brew.sh）
  执行：bash run.sh
  说明：将当前目录写入 LD_LIBRARY_PATH。
