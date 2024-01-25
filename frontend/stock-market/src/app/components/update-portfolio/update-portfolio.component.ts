import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PortfolioService } from '../../services/portfolio.service';
import { Portfolio } from '../../models/portfolio';

@Component({
  selector: 'app-update-portfolio',
  templateUrl: './update-portfolio.component.html',
  styleUrl: './update-portfolio.component.css'
})
export class UpdatePortfolioComponent implements OnInit {
  portfolioForm!: FormGroup;
  portfolioId!: number;
  portfolio!: Portfolio;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private portfolioService: PortfolioService,
  ) { }

  ngOnInit(): void {

    const idParam: string | null = this.route.snapshot.paramMap.get('id');
      
    if (idParam !== null) {
      this.portfolioId = parseInt(idParam, 10);
      this.portfolioForm = this.fb.group({
        ownerName: ['', Validators.required],
        bankName: ['', Validators.required],
        bankBalance: [null, [Validators.required, Validators.min(0)]],
        riskTolerance: [null, [Validators.required, Validators.min(0)]],
        investmentStrategy: ['', Validators.required]
      });

      this.loadPortfolioData();
    }
  }

  loadPortfolioData() {
    this.portfolioService.getPortfolio(this.portfolioId).subscribe((data: any) => 
    {
      this.portfolio = data.portfolio
      this.portfolioForm.patchValue(this.portfolio);
      console.log(this.portfolio)

    })
  }

  onSubmit() {
    if (this.portfolioForm.valid) {
      const formData = this.portfolioForm.value;

      this.portfolioService.updatePortfolio(this.portfolioId, formData).subscribe((rep)=>console.log("1"+rep))

      console.log("test");
      this.router.navigate(['/portfolio', this.portfolioId]);
    }
  }
}
