---
title: Performance Profiler
weight: 15
chapter: false
---

<div class="imgBox"><div>
	<img src="/images/Profiler.png" />
	<span>Performance Profiler</span>
</div></div>

The profiler automatically collects data about all function calls done by the code, as well as the number of clock cycles spent in each respective function.  

Using the profiler makes it is easy to find the bottlenecks in a game's code, which can help code optimization efforts.

If you are familiar with Visual Studio's profiler, these columns should be familiar:  

* **Call Count**: The number of times this function was called during profiling
* **Inclusive Time (Cyc)**: The amount of CPU cycles spent within this function (including the cycles spent by all functions called by this function)
* **Inclusive Time (%)**: The relative portion of CPU time spent within this function (including the time spent by all functions called by this function)
* **Exclusive Time (Cyc)**: The amount of CPU cycles spent within this function (functions called by this function are excluded)
* **Exclusive Time (%)**: The relative portion of CPU time spent within this function (functions called by this function are excluded)
* **Call Count**: The number of times this function was called during profiling

Use the `Reset` button to reset the profiler's data -- use this when you want to profile a specific portion of the execution.