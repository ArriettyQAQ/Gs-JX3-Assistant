import { Component, OnInit } from '@angular/core';
import { faDownload, faPlay } from '@fortawesome/free-solid-svg-icons';
import { AutomatorService } from '../../../services/automator/automator.service';

@Component({
  selector: 'app-automator-setup',
  templateUrl: './automator-setup.component.html',
  styleUrls: ['./automator-setup.component.scss'],
})
export class AutomatorSetupComponent implements OnInit {
  constructor(private automatorService: AutomatorService) {}

  ngOnInit(): void {}
  faDownload = faDownload;
  faPlay = faPlay;

  public download() {
    this.automatorService.download();
  }
}
