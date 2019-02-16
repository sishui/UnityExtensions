
Localization
|
|____Development - 开发文件（发布后删除可减小体积，但不便于玩家制作自己的语言）
|    |
|    |____ExportTool
|    |    |
|    |    |____ExportTool.exe
|    |
|    |____Configuration.xlsx - 本地化配置表
|    |    |
|    |    |____Font(sheet)
|    |    |
|    |    |____Language(sheet)
|    |    |
|    |    |____Style(sheet)
|    |
|    |____Example.xlsx
|    |    |
|    |    |____Text(sheet)
|    |    |    |
|    |    |    |____Name(col)
|    |    |    |
|    |    |    |____Style(col)
|    |    |    |
|    |    |    |____zh-CN(col), en-US(col), ... - 任意列数的语言
|    |    |
|    |    |____Text2(sheet), Text3(sheet), ... - 任意数量、任意名称的表格
|    |
|    |____Menu.xlsx, Dialog.xlsx, ... - 任意数量的本地化表
|
|____Configuration, zh-CN, en-US, ... - 由导出工具导出的语言文件



____________________________________________________________________________________________________________



Q：我用的不是 Windows 系统，无法运行 ExportTool.exe 怎么办？

A：ExportTool 源代码在这里：