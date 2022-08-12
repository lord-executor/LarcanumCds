import { Component, Input, OnChanges, OnInit, SimpleChanges } from "@angular/core";
import mermaid from 'mermaid';


@Component({
  selector: 'app-markdown',
  template: '<div [innerHtml]="markdownContent"></div>'
})
export class MarkdownComponent implements OnInit, OnChanges {
  @Input()
  public markdownContent: string|null = null;

  public ngOnInit(): void {
    mermaid.initialize({});
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes['markdownContent'].currentValue) {
      Promise.resolve().then(() => mermaid.init('.mermaid'));
    }
  }
}
