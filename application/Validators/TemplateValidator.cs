using domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Validators
{
    public class TemplateValidator:AbstractValidator<Template>
    {
        public TemplateValidator() 
        {
            RuleFor(t=> t.entities).NotNull().WithMessage(ResourcesApp.MessageFieldEntities);
            RuleForEach(t => t.entities).ChildRules(cr =>
            {
                cr.RuleFor(i => i.name).NotEmpty().NotNull().WithMessage(ResourcesApp.MessageFieldEntityName);
            });

            RuleFor(t=> t.Client).NotNull().NotEmpty().WithMessage(ResourcesApp.MessaFieldTemplateClient);
            RuleFor(t => t.origins).NotNull().NotEmpty().WithMessage(ResourcesApp.MessageFieldTemplateOrigin);
            RuleFor(t => t.transformationKey).NotNull().NotEmpty().WithMessage(ResourcesApp.MessageFieldTransformationKey);
        }
       
    }
}
