using System.Diagnostics;

namespace AdventOfCode.Puzzles;

public abstract class PuzzleBaseWithProgress : IPuzzle
{
    int _progress = 0;
    string? _executionInfo;
    bool _progressDrawn;

    public async Task Solve(string input)
    {
        Console.WriteLine(GetType().Name);
        
        var stopwatch = new Stopwatch();
        var cancellationTokenSource = new CancellationTokenSource();

        stopwatch.Start();

        var solutionTask = Task.Run(() => Solution(input));
        var progressTask = ShowProgress(cancellationTokenSource.Token);

        var result = await solutionTask;
        stopwatch.Stop();
        
        cancellationTokenSource.Cancel();
        await progressTask;


        Console.WriteLine($"Result: {result}");
        Console.WriteLine($"Time elapsed: {Math.Round(stopwatch.Elapsed.TotalSeconds, 3)}s.");
    }

    async Task ShowProgress(CancellationToken cancellationToken)
    {
        try 
        {
            var spinerPos = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                if (_progress > 0)
                {
                    DrawProgressBar(_progress);
                    _progressDrawn = true;
                }
                else
                {
                    char[] spinner = { '|', '/', '-', '\\' };
                    Console.Write($"\rProcessing... {spinner[spinerPos++ % 4]}");
                }

                Console.Write($" | {stopwatch.Elapsed:mm\\:ss}");

                if (_executionInfo != null)
                {
                    Console.Write($" | {_executionInfo}");
                }

                await Task.Delay(100, cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
            if (_progressDrawn)
            {
                DrawProgressBar(100);
            }

            Console.WriteLine();
        }
    }

    static void DrawProgressBar(int progress)
    {
        int barLength = 50;
        int filledLength = progress * barLength / 100;
        string bar = new string('█', filledLength) + new string('░', barLength - filledLength);
        Console.Write($"\rProcessing... [{bar}] {progress}%");
    }

    protected virtual bool ReportProgress { get; set; } = false;

    protected void SetProgress(int progress)
    {
        if (progress >= 0 && progress <= 100)
        {
            _progress = progress;
        }
    }

    protected void SetExecutionInfo(string info)
    {
        _executionInfo = info;
    }

    protected void SetExecutionInfo()
    {
        _executionInfo = new string(Enumerable.Repeat(' ', _executionInfo?.Length ?? 0).ToArray()); 
    }

    protected abstract string Solution(string input);
}
