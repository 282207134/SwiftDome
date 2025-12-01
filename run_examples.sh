#!/bin/bash

# ============================================
# Swift 学习项目 - 运行所有示例
# ============================================

echo "======================================"
echo "  Swift 学习项目 - 示例运行器"
echo "======================================"
echo ""

# 检查 Swift 是否安装
if ! command -v swift &> /dev/null; then
    echo "❌ 错误：未找到 Swift。请先安装 Swift。"
    echo "参考：https://swift.org/download/"
    exit 1
fi

echo "✅ Swift 版本："
swift --version
echo ""

# 运行所有示例的函数
run_directory() {
    local dir=$1
    local name=$2
    
    echo "======================================"
    echo "  $name"
    echo "======================================"
    echo ""
    
    if [ ! -d "$dir" ]; then
        echo "⚠️  目录不存在：$dir"
        echo ""
        return
    fi
    
    for file in "$dir"/*.swift; do
        if [ -f "$file" ]; then
            echo ">>> 运行: $(basename "$file")"
            echo "--------------------------------------"
            swift "$file"
            echo ""
            echo "✅ 完成: $(basename "$file")"
            echo ""
        fi
    done
}

# 主菜单
echo "请选择运行模式："
echo "1) 运行所有示例"
echo "2) 按章节运行"
echo "3) 运行综合项目"
echo "4) 退出"
echo ""
read -p "请输入选项 (1-4): " choice

case $choice in
    1)
        echo ""
        echo "开始运行所有示例..."
        echo ""
        
        run_directory "Sources/01-Basics" "01 · 基础知识"
        run_directory "Sources/02-ControlFlow" "02 · 控制流"
        run_directory "Sources/03-Functions" "03 · 函数"
        run_directory "Sources/04-Closures" "04 · 闭包"
        run_directory "Sources/05-Collections" "05 · 集合类型"
        run_directory "Sources/06-Structures" "06 · 结构体和类"
        run_directory "Sources/07-Properties" "07 · 属性"
        run_directory "Sources/08-Methods" "08 · 方法"
        run_directory "Sources/09-Inheritance" "09 · 继承"
        run_directory "Sources/10-Protocols" "10 · 协议"
        run_directory "Sources/11-Generics" "11 · 泛型"
        run_directory "Sources/12-ErrorHandling" "12 · 错误处理"
        run_directory "Sources/13-Concurrency" "13 · 并发编程"
        run_directory "Sources/14-Extensions" "14 · 扩展"
        run_directory "Sources/15-Advanced" "15 · 高级特性"
        
        echo "======================================"
        echo "  所有示例运行完成！"
        echo "======================================"
        ;;
    
    2)
        echo ""
        echo "可用章节："
        echo "1)  基础知识"
        echo "2)  控制流"
        echo "3)  函数"
        echo "4)  闭包"
        echo "5)  集合类型"
        echo "6)  结构体和类"
        echo "7)  属性"
        echo "8)  方法"
        echo "9)  继承"
        echo "10) 协议"
        echo "11) 泛型"
        echo "12) 错误处理"
        echo "13) 并发编程"
        echo "14) 扩展"
        echo "15) 高级特性"
        echo ""
        read -p "请选择章节 (1-15): " chapter
        
        case $chapter in
            1) run_directory "Sources/01-Basics" "01 · 基础知识" ;;
            2) run_directory "Sources/02-ControlFlow" "02 · 控制流" ;;
            3) run_directory "Sources/03-Functions" "03 · 函数" ;;
            4) run_directory "Sources/04-Closures" "04 · 闭包" ;;
            5) run_directory "Sources/05-Collections" "05 · 集合类型" ;;
            6) run_directory "Sources/06-Structures" "06 · 结构体和类" ;;
            7) run_directory "Sources/07-Properties" "07 · 属性" ;;
            8) run_directory "Sources/08-Methods" "08 · 方法" ;;
            9) run_directory "Sources/09-Inheritance" "09 · 继承" ;;
            10) run_directory "Sources/10-Protocols" "10 · 协议" ;;
            11) run_directory "Sources/11-Generics" "11 · 泛型" ;;
            12) run_directory "Sources/12-ErrorHandling" "12 · 错误处理" ;;
            13) run_directory "Sources/13-Concurrency" "13 · 并发编程" ;;
            14) run_directory "Sources/14-Extensions" "14 · 扩展" ;;
            15) run_directory "Sources/15-Advanced" "15 · 高级特性" ;;
            *) echo "❌ 无效的选项" ;;
        esac
        ;;
    
    3)
        echo ""
        echo "可用项目："
        echo "1) TodoApp - 待办事项应用"
        echo "2) WeatherApp - 天气应用"
        echo ""
        read -p "请选择项目 (1-2): " project
        
        case $project in
            1)
                echo ""
                echo "======================================"
                echo "  TodoApp - 待办事项应用"
                echo "======================================"
                echo ""
                swift Examples/TodoApp/TodoApp.swift
                ;;
            2)
                echo ""
                echo "======================================"
                echo "  WeatherApp - 天气应用"
                echo "======================================"
                echo ""
                swift Examples/WeatherApp/WeatherApp.swift
                ;;
            *) echo "❌ 无效的选项" ;;
        esac
        ;;
    
    4)
        echo "再见！"
        exit 0
        ;;
    
    *)
        echo "❌ 无效的选项"
        exit 1
        ;;
esac

echo ""
echo "======================================"
echo "  感谢使用 Swift 学习项目！"
echo "======================================"
