using domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Validators
{
    public class TargetValidator : AbstractValidator<Target>
    {
        public TargetValidator()
        {
            RuleFor(t=> t.entities).NotNull().NotEmpty().WithMessage(Resources.MessageFieldTargetEntities);
            RuleForEach(t=> t.entities).ChildRules(cr => {
                cr.RuleFor(i => i.name).NotEmpty().NotNull().WithErrorCode(Resources.MessageFieldEntityName);
            });
            RuleFor(t => t.entityTarget).NotNull().NotEmpty().WithMessage(Resources.MessageFieldEntityTarget);
        }

    }
}
