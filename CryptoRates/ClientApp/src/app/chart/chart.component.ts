import { Component, OnInit, Input } from '@angular/core';
import * as CanvasJS from './canvasjs.min';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {
  @Input('chartId') chartId: string;
  @Input('prices') data: number[];

  constructor() { }
  
  ngOnInit() {

  }

  ngAfterViewInit() {
    let chartData = [];
    this.data.forEach(c => {
      chartData.push({ y: c });
    });
    let chart = new CanvasJS.Chart(this.chartId, {
      zoomEnabled: true,
      animationEnabled: true,
      axisX: {
        gridThickness: 0,
        tickLength: 0,
        lineThickness: 0,
        labelFormatter: function () {
          return " ";
        }
      },
      axisY: {
        minimum: Math.min.apply(null, this.data),
        maximum: Math.max.apply(null, this.data),
        gridThickness: 0,
        tickLength: 0,
        lineThickness: 0,
        labelFormatter: function () {
          return " ";
        }
      },
      data: [
        {
          type: "line",
          dataPoints: chartData
        }]
    });
    chart.render();
  }
}
