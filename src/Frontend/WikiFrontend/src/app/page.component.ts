import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LarcanumCdsService } from './larcanum-cds.service';

@Component({
  selector: 'app-page',
  template: '<h2>{{ content?.Title }}</h2> <app-markdown [markdownContent]="content?.Body"></app-markdown>'
})
export class PageComponent {
  public content: any | null = null;

  public constructor(
    private readonly route: ActivatedRoute,
    private readonly contentService: LarcanumCdsService
  ) {
    this.contentService.getBySlug(route.snapshot.url.join('/')).subscribe(content => {
      console.log("got home content", content);
      this.content = content;
    });
  }
}
