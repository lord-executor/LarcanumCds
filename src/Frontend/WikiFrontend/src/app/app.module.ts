import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LarcanumCdsService } from './larcanum-cds.service';
import { MarkdownComponent } from './markdown.component';
import { PageComponent } from './page.component';

@NgModule({
  declarations: [
    AppComponent,
    PageComponent,
    MarkdownComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [LarcanumCdsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
